using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using Neural.Cost;
using Neural.Perceptron;

namespace Neural.Training
{
    /// <summary>
    /// Class CostBase.
    /// </summary>
    public class DefaultCostGradient : ICostGradient
    {
        /// <summary>
        /// The cost function
        /// </summary>
        [NotNull]
        private readonly ICostFunction _costFunction;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCostGradient"/> class.
        /// </summary>
        /// <param name="costFunction">The cost function.</param>
        public DefaultCostGradient([NotNull] ICostFunction costFunction)
        {
            _costFunction = costFunction;
        }

        #region Cost Calculation

        /// <summary>
        /// Calculates the network's training cost.
        /// </summary>
        /// <param name="expectedOutput">The expected output, i.e. ground truth.</param>
        /// <param name="networkOutput">The network output.</param>
        /// <returns>System.Single.</returns>
        [DebuggerStepThrough]
        protected virtual float CalculateCost(Vector<float> expectedOutput, Vector<float> networkOutput)
        {
            return _costFunction.CalculateCost(expectedOutput, networkOutput);
        }

        /// <summary>
        /// Calculates the network's training cost.
        /// </summary>
        /// <param name="expectedOutput">The expected output, i.e. ground truth.</param>
        /// <param name="networkOutput">The network output.</param>
        /// <returns>System.Single.</returns>
        [DebuggerStepThrough]
        protected float CalculateCost(Vector<float> expectedOutput, FeedforwardResult networkOutput)
        {
            Debug.Assert(networkOutput.LayerType == LayerType.Output, "networkOutput.LayerType == LayerType.Output");
            return CalculateCost(expectedOutput, networkOutput.Output);
        }

        #endregion Cost Calculation

        #region Error Gradient

        /// <summary>
        /// Calculates the error gradient of a given layer.
        /// </summary>
        /// <remarks>
        /// This assumes a single training pass. If the gradients are aggregated over multiple training examples,
        /// the result needs to be scaled accordingly.
        /// </remarks>
        /// <param name="resultNode">The layer's feedforward result node.</param>
        /// <param name="error">The layer's output error.</param>
        /// <returns>ErrorGradient.</returns>
        [Pure, DebuggerStepThrough]
        private ErrorGradient CalculateErrorGradient([NotNull] LinkedListNode<FeedforwardResult> resultNode, BackpropagationResult error)
        {
            return CalculateErrorGradient(resultNode, error.WeightErrors);
        }

        /// <summary>
        /// Calculates the error gradient of a given layer.
        /// </summary>
        /// <remarks>
        /// This assumes a single training pass. If the gradients are aggregated over multiple training examples,
        /// the result needs to be scaled accordingly.
        /// </remarks>
        /// <param name="resultNode">The layer's feedforward result node.</param>
        /// <param name="weightErrors">The layer's output error.</param>
        /// <returns>ErrorGradient.</returns>
        [Pure]
        protected virtual ErrorGradient CalculateErrorGradient([NotNull] LinkedListNode<FeedforwardResult> resultNode, [NotNull] Vector<float> weightErrors)
        {
            // in order to calculate the gradient, we require the
            // output activations of the previous layer. In case the previous
            // layer happens to be the input layer, this will be the raw inputs.
            var layerInput = GetLayerInput(resultNode);

            // calculate the gradient
            var weightGradient = weightErrors.OuterProduct(layerInput);
            var biasGradient = weightErrors; // the bias input is always 1, so this is trivial

            return new ErrorGradient(weightGradient, biasGradient);
        }

        #endregion Error Gradient

        /// <summary>
        /// Calculates the cost and the gradient of the cost function given the training examples.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="lambda">The regularization parameter.</param>
        /// <returns>System.Single.</returns>
        [Pure, DebuggerStepThrough]
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
        protected virtual TrainingResult CalculateCostAndGradientUnregularized(Network network, IReadOnlyCollection<TrainingExample> trainingSet)
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
        protected virtual TrainingResult CalculateCostAndGradientUnregularized(Network network, TrainingExample example)
        {
            // prepare the output structures
            var gradients = new Dictionary<Layer, ErrorGradient>();

            // fetch the training data
            var input = example.GetInputs();
            var expectedOutput = example.GetOutputs();

            // perform a feed-forward pass and retrieve the intermediate results
            var feedforwardResults = network.Feedforward(input);
            Debug.Assert(network.LayerCount == feedforwardResults.Count, "layers.Count == feedforwardResults.Count");

            // we start with the output layer
            var layer = network.OutputLayer;
            var resultNode = feedforwardResults.Last;

            // Calculate the error of the network output regarding the wanted training output,
            // as well as the error gradient of the output layer.
            // The calculated error will also serve as an input to the layer loop below.
            var error = CalculateNetworkOutputError(feedforwardResults, expectedOutput);
            var outputGradient = CalculateErrorGradient(resultNode, error);
            gradients.Add(layer, outputGradient);

            // calculate the training cost
            var j = CalculateCost(expectedOutput, resultNode.Value);

            // as long as we do not hit the input layer, iterate backwards
            // through all hidden layers
            Debug.Assert(layer.Previous != null, "layer.Previous != null");
            while (layer.Previous.Type != LayerType.Input)
            {
                layer = layer.Previous;
                resultNode = resultNode.Previous;

                // I know it's true, you know it's true ...
                Debug.Assert(layer != null, "layer != null");
                Debug.Assert(resultNode != null, "resultNode != null");

                // obtain this layer's feedforward results
                var feedforwardResult = resultNode.Value;

                // errors on the hidden layer must be obtained through backpropagation
                var delta = layer.Backpropagate(
                    feeforwardResult: feedforwardResult,
                    outputErrors: error);

                // calculate the error gradient
                var hiddenGradient = CalculateErrorGradient(resultNode, delta);
                gradients.Add(layer, hiddenGradient);

                // store the error for the next iteration
                error = delta.WeightErrors;
            }

            return new TrainingResult(j, gradients);
        }

        #region Regularization

        /// <summary>
        /// Calculates the cost and the gradient of the cost function given the training examples.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="lambda">The regularization parameter.</param>
        /// <returns>System.Single.</returns>
        [Pure]
        private TrainingResult CalculateCostAndGradientRegularized([NotNull] Network network, [NotNull] IReadOnlyCollection<TrainingExample> trainingSet, float lambda)
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
