using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using Neural.Perceptron;

namespace Neural.Cost
{
    /// <summary>
    /// Class CostBase.
    /// </summary>
    public abstract class CostBase : ICostFunction
    {
        /// <summary>
        /// Calculates the cost and the gradient of the cost function given the training examples.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="lambda">The regularization parameter.</param>
        /// <returns>System.Single.</returns>
        [Pure]
        public TrainingResult CalculateCostAndGradient(Network network, IReadOnlyCollection<TrainingExample> trainingSet, float lambda)
        {
            return lambda > 0
                ? CalculateCostAndGradientRegularized(network, trainingSet, lambda)
                : CalculateCostAndGradientUnregularized(network, trainingSet);
        }

        /// <summary>
        /// Calculates the cost given the training examples.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="trainingSet">The training set.</param>
        /// <returns>System.Single.</returns>
        [Pure]
        public virtual TrainingResult CalculateCostAndGradientUnregularized(Network network, IReadOnlyCollection<TrainingExample> trainingSet)
        {
            // Map: Apply training method to each example
            var trainingResults = trainingSet.Select(example => CalculateCostAndGradientUnregularized(network, example));

            // initialize cost and accumulated gradients
            var gradientDictionary = new Dictionary<Layer, ErrorGradient>();
            var cost = 0.0F;

            // for each layer after the input, initialize the gradient
            // accumulation matrices to zero
            foreach (var layer in network.Skip(1))
            {
                Debug.Assert(layer.Type != LayerType.Input, "layer.Type != LayerType.Input");

                var gradient = ErrorGradient.EmptyFromLayer(layer);
                gradientDictionary.Add(layer, gradient);
            }

            // Reduce: merge the training examples
            foreach (var trainingResult in trainingResults)
            {
                // accumulate cost over all training examples
                cost += trainingResult.Cost;

                // iterate over all layer's error gradients of this training example
                // TODO: this is data parallel with respect to layers, so may run in parallel
                var trainingGradients = trainingResult.ErrorGradients;
                foreach (var trainingGradient in trainingGradients)
                {
                    var layer = trainingGradient.Key;
                    var gradient = trainingGradient.Value;

                    // accumulate gradients over all training examples
                    gradientDictionary[layer] += gradient;
                }
            }

            // scale cost and gradients by the number of training examples
            var inverseExampleCount = 1.0F / trainingSet.Count;
            cost *= inverseExampleCount;
            gradientDictionary = gradientDictionary.AsParallel()
                .ToDictionary(item => item.Key, item => item.Value * inverseExampleCount);

            return new TrainingResult(cost, gradientDictionary);
        }

        /// <summary>
        /// Calculates the (unregularized) cost and gradient given a single training example.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="example">The training example.</param>
        /// <returns>TrainingResult.</returns>
        [Pure]
        public abstract TrainingResult CalculateCostAndGradientUnregularized(Network network, TrainingExample example);

        #region Regularization

        /// <summary>
        /// Calculates the cost and the gradient of the cost function given the training examples.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="lambda">The regularization parameter.</param>
        /// <returns>System.Single.</returns>
        [Pure]
        public TrainingResult CalculateCostAndGradientRegularized([NotNull] Network network, [NotNull] IReadOnlyCollection<TrainingExample> trainingSet, float lambda)
        {
            var unregularizedResult = CalculateCostAndGradientUnregularized(network, trainingSet);
            var count = trainingSet.Count;

            // regularize the training cost
            var layers = network.Skip(1);
            var sumOfSquaredWeights = layers.AsParallel().Sum(layer => layer.Weights.Map(v => v * v).RowSums().Sum());
            var costRegularization = lambda / (2 * count) * sumOfSquaredWeights;
            var regularizedCost = unregularizedResult.Cost + costRegularization;

            // regularization of gradients is obtained by simply adding
            // the scaled weights to the gradient
            var unregularizedGradients = unregularizedResult.ErrorGradients;
            var regularizationFactor = lambda / count;
            var regularizedGradient = unregularizedGradients.AsParallel().ToDictionary(
                entry => entry.Key,
                entry => RegularizeErrorGradient(entry.Key, entry.Value, regularizationFactor));

            // return the regularized result
            return new TrainingResult(regularizedCost, regularizedGradient);
        }

        /// <summary>
        /// Regularizes the error gradient.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="unregularizedGradient">The unregularized gradient.</param>
        /// <param name="regularizationFactor">The regularization factor (lambda/no. of training examples).</param>
        /// <returns>ErrorGradient.</returns>
        private static ErrorGradient RegularizeErrorGradient([NotNull] Layer layer, ErrorGradient unregularizedGradient, float regularizationFactor)
        {
            var regularizedWeightGradient = unregularizedGradient.Weight + regularizationFactor * layer.Weights;
            return new ErrorGradient(regularizedWeightGradient, unregularizedGradient.Bias);
        }

        #endregion Regularization

        #region Helper functions

        /// <summary>
        /// Calculates the network output error.
        /// </summary>
        /// <param name="feedforwardResults">The feedforward results.</param>
        /// <param name="expectedOutput">The expected output.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        [Pure, NotNull]
        protected static Vector<float> CalculateNetworkOutputError([NotNull] LinkedList<FeedforwardResult> feedforwardResults, [NotNull] Vector<float> expectedOutput)
        {
            var outputLayer = feedforwardResults.Last.Value;
            var networkOutput = outputLayer.Output;
            var error = networkOutput - expectedOutput;
            return error;
        }

        /// <summary>
        /// Gets the input values of the <see cref="Layer"/> that belongs to the <paramref name="resultNode"/>.
        /// </summary>
        /// <param name="resultNode">The result node.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        [Pure, NotNull]
        protected static Vector<float> GetLayerInput([NotNull] LinkedListNode<FeedforwardResult> resultNode)
        {
            Debug.Assert(resultNode.Value.LayerType != LayerType.Input, "resultNode.Value.LayerType != LayerType.Input");

            // obtain the previous node
            var previousLayerNode = resultNode.Previous;
            Debug.Assert(previousLayerNode != null, "previousLayerNode != null");

            // the previous layer's output are the inputs of the layer given to this function
            var previousLayer = previousLayerNode.Value;
            return previousLayer.Output;
        }

        #endregion Helper functions
    }
}
