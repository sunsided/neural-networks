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
            var exampleCount = trainingSet.Count;
            foreach (var example in trainingSet)
            {
                CalculateCostAndGradientUnregularized(example);

                // TODO: Accumulate cost over all training examples
                // TODO: Scale host over all training examples

                // TODO: Accumulate gradients over all training examples
                // TODO: Scale gradients by the number of training examples
                // TODO: Add regularization

            } // for(examples)

            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the (unregularized) cost and gradient given a single training example.
        /// </summary>
        /// <param name="example">The training example.</param>
        private void CalculateCostAndGradientUnregularized(TrainingExample example)
        {
            var layers = _layers;

            // fetch the training data
            var input = Vector<float>.Build.SparseOfEnumerable(example.Inputs);
            var expectedOutput = Vector<float>.Build.SparseOfEnumerable(example.Outputs);

            // perform a feed-forward pass and retrieve the intermediate results
            var feedforwardResults = CalculateInternal(input);
            Debug.Assert(layers.Count == feedforwardResults.Count, "layers.Count == feedforwardResults.Count");

            // we start with the output layer
            var layer = OutputLayer;
            var resultNode = feedforwardResults.Last;

            // Calculate the error of the network output regarding the wanted training output,
            // as well as the error gradient of the output layer.
            // The calculated error will also serve as an input to the layer loop below.
            var error = CalculateNetworkOutputError(feedforwardResults, expectedOutput);
            var outputGradient = CalculateErrorGradient(resultNode, error);

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
                var layerOutput = resultNode.Value;

                // errors on the hidden layer must be obtained through backpropagation
                var z = layerOutput.WeightedInputs;
                var a = layerOutput.Output;
                var d = layer.Backpropagate(
                    layerResult: layerOutput,
                    outputErrors: error);

                // store the error for the next iteration
                error = d.WeightingErrors;

                // calculate the error gradient
                var hiddenGradient = CalculateErrorGradient(resultNode, error);
            }
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
        private static ErrorGradient CalculateErrorGradient([NotNull] LinkedListNode<FeedforwardResult> resultNode, [NotNull] Vector<float> error)
        {
            // in order to calculate the gradient, we require the
            // output activations of the previous layer. In case the previous
            // layer happens to be the input layer, this will be the raw inputs.
            var layerInput = GetLayerInput(resultNode);

            // calculate the gradient
            var weightGradient = error.OuterProduct(layerInput);
            var biasGradient = error; // the bias input is always 1, so this is trivial

            return new ErrorGradient(weightGradient, biasGradient);
        }

        /// <summary>
        /// Gets the input values of the <see cref="Layer"/> that belongs to the <paramref name="resultNode"/>.
        /// </summary>
        /// <param name="resultNode">The result node.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        [NotNull]
        private static Vector<float> GetLayerInput([NotNull] LinkedListNode<FeedforwardResult> resultNode)
        {
            Debug.Assert(resultNode.Value.LayerType != LayerType.Input, "resultNode.Value.LayerType != LayerType.Input");

            // obtain the previous node
            var previousLayerNode = resultNode.Previous;
            Debug.Assert(previousLayerNode != null, "previousLayerNode != null");

            // the previous layer's output are the inputs of the layer given to this function
            var previousLayer = previousLayerNode.Value;
            return previousLayer.Output;
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
