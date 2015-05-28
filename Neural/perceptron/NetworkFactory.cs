using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace Neural.Perceptron
{
    /// <summary>
    /// Class PerceptronNetworkFactory. This class cannot be inherited.
    /// </summary>
    sealed class NetworkFactory
    {
        /// <summary>
        /// Creates the specified input layer configuration.
        /// </summary>
        /// <param name="inputLayerConfiguration">The input layer configuration.</param>
        /// <param name="outputLayerConfiguration">The output layer configuration.</param>
        /// <returns>PerceptronNetwork.</returns>
        public PerceptronNetwork Create(LayerConfiguration inputLayerConfiguration, LayerConfiguration outputLayerConfiguration)
        {
            var layerConfigurations = new[]
                                      {
                                          inputLayerConfiguration,
                                          outputLayerConfiguration
                                      };

            return Create(layerConfigurations);
        }

        /// <summary>
        /// Creates the specified input layer configuration.
        /// </summary>
        /// <param name="inputLayerConfiguration">The input layer configuration.</param>
        /// <param name="hiddenLayerConfigurations">The hidden layer configurations.</param>
        /// <param name="outputLayerConfiguration">The output layer configuration.</param>
        /// <returns>PerceptronNetwork.</returns>
        public PerceptronNetwork Create(LayerConfiguration inputLayerConfiguration, [NotNull] IReadOnlyList<LayerConfiguration> hiddenLayerConfigurations, LayerConfiguration outputLayerConfiguration)
        {
            var layerConfigurations = new List<LayerConfiguration>(hiddenLayerConfigurations.Count + 2);
            layerConfigurations.Add(inputLayerConfiguration);
            layerConfigurations.AddRange(hiddenLayerConfigurations);
            layerConfigurations.Add(outputLayerConfiguration);

            return Create(layerConfigurations);
        }

        /// <summary>
        /// Creates the specified input layer configuration.
        /// </summary>
        /// <param name="layerConfigurations">The layer configurations.</param>
        /// <returns>PerceptronNetwork.</returns>
        private PerceptronNetwork Create<T>([NotNull] T layerConfigurations)
            where T : IEnumerable<LayerConfiguration>
        {
            // Prepare a linked list of perceptron layers
            var layerList = new LinkedList<Layer>();

            var inputLayer = layerConfigurations.First();
            var remainingLayers = layerConfigurations.Skip(1);

            // As the input layer has no previous layer, we initialize this as null.
            Layer previousLayer = null;
            var previousNextLayer = new WeakReference<Layer>(null);

            // This value encodes the number of neurons in the previous layer
            // that act as an input to each perceptron within this layer.
            // For the input layer, each input is directly assigned;
            // we encode this as one (virtual) input neuron per perceptron.
            var inputNeurons = inputLayer.NeuronCount;

            // We now iterate over all configurations and create weight vectors
            // for each perceptron according to the number of input neurons, where
            // each weight is initialized with a random value.
            // For efficient calculation, the weights of all nerons in a given layer
            // are stored as rows of a weight matrix.
            foreach (var layerConfiguration in remainingLayers)
            {
                var layerNeurons = layerConfiguration.NeuronCount;
                var activation = layerConfiguration.ActivationFunction;

                var biasVector = Vector<float>.Build.Random(layerNeurons);
                var weightMatrix = Matrix<float>.Build.Random(layerNeurons, inputNeurons);

                var nextLayer = new WeakReference<Layer>(null);

                var layer = new Layer(previousLayer, nextLayer, biasVector, weightMatrix, activation);
                layerList.AddLast(layer);

                // update the "next layer" reference in the previous layer.
                // if this is the first iteration, this update will do nothing as
                // the reference is not yet in used.
                previousNextLayer.SetTarget(layer);

                // now we update the reference to the new instance created above
                // so that the next iteration can update it accordingly.
                previousNextLayer = nextLayer;

                // We now store the number of neurons in this layer
                // as the number of input neurons of the next layer
                // and rewire the reference of the previous layer.
                inputNeurons = layerNeurons;
                previousLayer = layer;
            }

            return new PerceptronNetwork(layerList);
        }
    }
}
