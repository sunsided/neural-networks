using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace Neural.Perceptron
{
    /// <summary>
    /// A perceptron network.
    /// </summary>
    sealed class PerceptronNetwork
    {
        /// <summary>
        /// The perceptron layers
        /// </summary>
        [NotNull]
        private readonly LinkedList<Layer> _layers;

        /// <summary>
        /// Gets the number of input neurons.
        /// </summary>
        /// <value>The number of input neurons.</value>
        public int InputNeuronCount
        {
            [Pure]
            get
            {
                var layer = _layers.First.Value;
                return layer.NeuronCount;
            }
        }

        /// <summary>
        /// Gets the number of output neurons.
        /// </summary>
        /// <value>The number of input neurons.</value>
        public int OutputNeuronCount
        {
            [Pure]
            get
            {
                var layer = _layers.Last.Value;
                return layer.NeuronCount;
            }
        }

        /// <summary>
        /// Gets the input layer.
        /// </summary>
        /// <value>The input layer.</value>
        protected Layer InputLayer
        {
            [Pure, NotNull]
            get { return _layers.First.Value; }
        }

        /// <summary>
        /// Gets the output layer.
        /// </summary>
        /// <value>The output layer.</value>
        protected Layer OutputLayer
        {
            [Pure, NotNull]
            get { return _layers.Last.Value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerceptronNetwork"/> class.
        /// </summary>
        /// <param name="layers">The perceptron layers.</param>
        public PerceptronNetwork([NotNull] LinkedList<Layer> layers)
        {
            _layers = layers;
        }

        /// <summary>
        /// Calculates the outputs given the specified <paramref name="inputs"/>.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        [Pure, NotNull]
        public IReadOnlyList<float> Calculate([NotNull] IReadOnlyList<float> inputs)
        {
            var inputVector = Vector<float>.Build.SparseOfEnumerable(inputs);
            var outputVector = CalculateInternalFast(inputVector);
            return outputVector.ToArray();
        }

        /// <summary>
        /// Calculates the network outputs given the specified <paramref name="input"/>.
        /// </summary>
        /// <param name="input">The inputs.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        [Pure, NotNull]
        private Vector<float> CalculateInternalFast([NotNull] Vector<float> input)
        {
            // Starting with the given inputs as the first hidden layer's activation,
            // iterate through all layers and calculate the next layer's activations.
            // The resulting activation of the last layer are the outputs.
            return _layers.Aggregate(input, (layerInput, layer) => layer.Feedforward(layerInput).Output);
        }

        /// <summary>
        /// Performs a feed-forward pass through the network and stores the results of each layer.
        /// </summary>
        /// <param name="input">The inputs.</param>
        /// <returns>LinkedList&lt;Layer.FeedforwardResult&gt;.</returns>
        [Pure, NotNull]
        private LinkedList<FeedforwardResult> CalculateInternal([NotNull] Vector<float> input)
        {
            var layers = _layers;

            // prepare a list of all layer's results
            var feedforwardResults = new LinkedList<FeedforwardResult>();

            // run a forward propagation step and keep track of all intermediate reults
            var nextInput = input;
            foreach (var layer in layers)
            {
                // perform the feedforward iteration
                var layerInput = nextInput;
                var layerResult = layer.Feedforward(layerInput);
                nextInput = layerResult.Output;

                // the input layer must never change the input values,
                // it is here only for cosmetics.
                Debug.Assert((layer.Type != LayerType.Input) || layerResult.Output.Equals(layerInput), "(layer.Type != LayerType.Input) || layerResult.Output.Equals(layerInput)");

                // store the intermediate result
                feedforwardResults.AddLast(layerResult);
            }
            return feedforwardResults;
        }

        /// <summary>
        /// Trains the network using the given <paramref name="examples" />.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        public void Train([NotNull] IReadOnlyCollection<TrainingExample> trainingSet)
        {
            CalculateCostAndGradient(trainingSet);
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
            var firstPart = expectedOutput*logOutput;

            var logInvOutput = networkOutput.Map(v => (float)Math.Log(1 - v));
            var secondPart = (1-expectedOutput) * logInvOutput;

            return -firstPart - secondPart;
        }

        /// <summary>
        /// Calculates the cost given the training examples.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <returns>System.Single.</returns>
        /// <exception cref="NotImplementedException"></exception>
        private float CalculateCostAndGradient([NotNull] IReadOnlyCollection<TrainingExample> trainingSet)
        {
            var layers = _layers;
            var layerCount = layers.Count;

            var exampleCount = trainingSet.Count;
            foreach (var example in trainingSet)
            {
                var input = Vector<float>.Build.SparseOfEnumerable(example.Inputs);
                var expectedOutput = Vector<float>.Build.SparseOfEnumerable(example.Outputs);

                // perform a feed-forward pass and retrieve the intermediate results
                var feedforwardResults = CalculateInternal(input);
                Debug.Assert(layers.Count == feedforwardResults.Count, "layers.Count == feedforwardResults.Count");

                // calculate the error of the network output regarding the wanted training output
                var networkOutputError = CalculateNetworkOutputError(feedforwardResults, expectedOutput);
                var error = networkOutputError;

                // work it ... manually
                var outputLayer = OutputLayer;
                var hiddenLayer = outputLayer.Previous;

                var networkOutput = feedforwardResults.Last.Value;
                var hiddenOutput = feedforwardResults.Last.Previous.Value;
                var inputOutput = feedforwardResults.First.Value;

                // errors on the output layer can be calculated directly
                var z3 = networkOutput.WeightedInputs;
                var a3 = networkOutput.Output;
                var d3 = networkOutputError;

                // errors on the hidden layer must be obtained through backpropagation
                var z2 = hiddenOutput.WeightedInputs;
                var a2 = hiddenOutput.Output;
                var d2 = hiddenLayer.Backpropagate(
                    layerResult: hiddenOutput,
                    outputErrors:d3);

                // outputs on the input layer are required for gradient calculation
                var a1 = inputOutput.Output; // i.e. the training inputs

                // backpropagation stops at the first hidden layer, since the
                // input layer cannot be changed

                var outputLayerWeightGradient = d3.OuterProduct(a2);
                var outputLayerBiasGradient = networkOutputError; // the bias always has linear influence on the error

                var hiddenLayerWeightGradient = d2.WeightingErrors.OuterProduct(a1);
                var hiddenLayerBiasGradient = d2.WeightingErrors; // the bias always has linear influence on the error

                // TODO: Accumulate gradients over all training examples
                // TODO: Scale gradients by the number of training examples
                // TODO: Add regularization

                var j = CalculateCost(expectedOutput, networkOutput.Output);

                throw new NotImplementedException("gradient calculation from weight errors not implemented");




                // run the backward propagation steps
                // we start with the last node and iterate until we reach the first
                // hidden layer. The input layer is left out because there is no gradient
                // to correct, as inputs are what they are.
                var layerNodeOfCurrentLayer = layers.Last;
                var resultNodeOfPreviousLayer = feedforwardResults.Last.Previous;
                for (int layerIndex = 0; layerIndex < layerCount-1 /* -1 because we already know the output error as well */; ++layerIndex)
                {
                    Debug.Assert(layerNodeOfCurrentLayer != null, "layerNode != null");
                    Debug.Assert(resultNodeOfPreviousLayer != null, "resultNode != null");

                    // based on the current layer's matrix and the previous layer's
                    // weighted inputs (not output activations!) we determine the error
                    // of the previous (!) layer
                    var previousLayerResults = resultNodeOfPreviousLayer.Value;
                    var currentLayer = layerNodeOfCurrentLayer.Value;

                    var z = previousLayerResults.WeightedInputs;
                    var previousLayerError = currentLayer.Backpropagate(z, error);

                    // set the error for the next recursion
                    error = previousLayerError.WeightingErrors;

                    // move to the previous layer
                    layerNodeOfCurrentLayer = layerNodeOfCurrentLayer.Previous;
                    resultNodeOfPreviousLayer = resultNodeOfPreviousLayer.Previous;
                }
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the network output error.
        /// </summary>
        /// <param name="feedforwardResults">The feedforward results.</param>
        /// <param name="expectedOutput">The expected output.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        private static Vector<float> CalculateNetworkOutputError([NotNull] LinkedList<FeedforwardResult> feedforwardResults, [NotNull] Vector<float> expectedOutput)
        {
            var outputLayer = feedforwardResults.Last.Value;
            var networkOutput = outputLayer.Output;
            var error = networkOutput - expectedOutput;
            return error;
        }
    }
}
