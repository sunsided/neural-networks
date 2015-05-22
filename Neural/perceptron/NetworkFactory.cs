using System.Collections.Generic;
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
        /// <param name="hiddenLayerConfigurations">The hidden layer configurations.</param>
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
            // This value encodes the number of neurons in the previous layer
            // that act as an input to each perceptron within this layer.
            // For the input layer, each input is directly assigned; 
            // we encode this as one (virtual) input neuron per perceptron.
            var inputNeurons = 1;

            // We now iterate over all configurations and create weight vectors
            // for each perceptron according to the number of input neurons, where
            // each weight is initialized with a random value.
            // For efficient calculation, the weights of all nerons in a given layer 
            // are stored as rows of a weight matrix.
            foreach (var layerConfiguration in layerConfigurations)
            {
                var layerNeurons = layerConfiguration.NeuronCount;
                var weightMatrix = Matrix<float>.Build.Random(layerNeurons, inputNeurons);

                var activation = layerConfiguration.ActivationFunction;

                // We now store the number of neurons in this layer 
                // as the number of input neurons of the next layer.
                inputNeurons = layerNeurons;
            }
        }
    }
}
