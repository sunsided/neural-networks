using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using Neural.Perceptron;

namespace Neural.Cost
{
    /// <summary>
    /// Logistic regression-like multivariate cost function.
    /// </summary>
    sealed class LogisticCost : CostBase
    {
        #region Actual cost calculation (no gradients)

        /// <summary>
        /// Calculates the network's training cost.
        /// </summary>
        /// <param name="expectedOutput">The expected output, i.e. ground truth.</param>
        /// <param name="networkOutput">The network output.</param>
        /// <returns>System.Single.</returns>
        private float CalculateCost(Vector<float> expectedOutput, FeedforwardResult networkOutput)
        {
            Debug.Assert(networkOutput.LayerType == LayerType.Output, "networkOutput.LayerType == LayerType.Output");
            return CalculateCost(expectedOutput, networkOutput.Output);
        }

        /// <summary>
        /// Calculates the network's training cost.
        /// </summary>
        /// <param name="expectedOutput">The expected output, i.e. ground truth.</param>
        /// <param name="networkOutput">The network output.</param>
        /// <returns>System.Single.</returns>
        private float CalculateCost(Vector<float> expectedOutput, Vector<float> networkOutput)
        {
            var logOutput = networkOutput.Map(v => (float)Math.Log(v));
            var firstPart = expectedOutput * logOutput;

            var logInvOutput = networkOutput.Map(v => (float)Math.Log(1 - v)); // BUG: this will blow up if the network output is actually 1 (or larger)
            var secondPart = (1 - expectedOutput) * logInvOutput;

            return -firstPart - secondPart;
        }

        #endregion Actual cost calculation (no gradients)

        #region Gradient calculation

        /// <summary>
        /// Calculates the (unregularized) cost and gradient given a single training example.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="example">The training example.</param>
        /// <returns>TrainingResult.</returns>
        [Pure]
        public override TrainingResult CalculateCostAndGradientUnregularized(Network network, TrainingExample example)
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
            var error = network.CalculateNetworkOutputError(feedforwardResults, expectedOutput);
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
        [Pure]
        private static ErrorGradient CalculateErrorGradient([NotNull] LinkedListNode<FeedforwardResult> resultNode, BackpropagationResult error)
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
        private static ErrorGradient CalculateErrorGradient([NotNull] LinkedListNode<FeedforwardResult> resultNode, [NotNull] Vector<float> weightErrors)
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

        #endregion Gradient calculation
    }
}
