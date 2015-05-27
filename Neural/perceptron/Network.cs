using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
        /// Calculates the network outputs given the specified <paramref name="inputs"/>.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        [Pure, NotNull]
        private Vector<float> CalculateInternalFast([NotNull] Vector<float> inputs)
        {
            // Starting with the given inputs as the first hidden layer's activation,
            // iterate through all layers and calculate the next layer's activations.
            // The resulting activation of the last layer are the outputs.
            return _layers.Aggregate(inputs, (activations, layer) => layer.Feedforward(activations).Activation);
        }

        /// <summary>
        /// Performs a feed-forward pass through the network and stores the results of each layer.
        /// </summary>
        /// <param name="input">The inputs.</param>
        /// <returns>LinkedList&lt;Layer.FeedforwardResult&gt;.</returns>
        [Pure, NotNull]
        private LinkedList<Layer.FeedforwardResult> CalculateInternal([NotNull] Vector<float> input)
        {
            var layers = _layers;

            // prepare a list of all layer's results
            var feedforwardResults = new LinkedList<Layer.FeedforwardResult>();

            // run a forward propagation step and keep track of all intermediate reults
            var layerInput = input;
            foreach (var layer in layers)
            {
                // perform the feedforward iteration
                var result = layer.Feedforward(layerInput);
                layerInput = result.Activation;

                // store the intermediate result
                feedforwardResults.AddLast(result);
            }
            return feedforwardResults;
        }

        /// <summary>
        /// Trains the network using the given <paramref name="examples" />.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        public void Train([NotNull] IReadOnlyCollection<TrainingExample> trainingSet)
        {
            CalculateCost(trainingSet);
        }

        /// <summary>
        /// Calculates the cost given the training examples.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <returns>System.Single.</returns>
        /// <exception cref="NotImplementedException"></exception>
        private float CalculateCost([NotNull] IReadOnlyCollection<TrainingExample> trainingSet)
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
                var outputLayer = feedforwardResults.Last.Value;
                var networkOutput = outputLayer.Activation;
                var error = networkOutput - expectedOutput;

                // run the backward propagation steps
                // we start with the last node and iterate until we reach the first
                // hidden layer. The input layer is left out because there is no gradient
                // to correct, as inputs are what they are.
                var layerNodeOfCurrentLayer = layers.Last;
                var resultNodeOfPreviousLayer = feedforwardResults.Last.Previous;
                for (int layerIndex = 0; layerIndex < layerCount-1; ++layerIndex)
                {
                    Debug.Assert(layerNodeOfCurrentLayer != null, "layerNode != null");
                    Debug.Assert(resultNodeOfPreviousLayer != null, "resultNode != null");

                    // based on the current layer's matrix and the previous layer's
                    // weighted inputs (not output activations!) we determine the error
                    // of the previous (!) layer
                    var z = resultNodeOfPreviousLayer.Value.Input;
                    var layer = layerNodeOfCurrentLayer.Value;
                    var previousLayerError = layer.Backpropagate(error, z);

                    // set the error for the next recursion
                    error = previousLayerError;

                    // move to the previous layer
                    layerNodeOfCurrentLayer = layerNodeOfCurrentLayer.Previous;
                    resultNodeOfPreviousLayer = resultNodeOfPreviousLayer.Previous;
                }
            }

            throw new NotImplementedException();
        }
    }
}
