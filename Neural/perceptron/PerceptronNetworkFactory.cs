using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Neural.perceptron
{
    /// <summary>
    /// Class PerceptronNetworkFactory. This class cannot be inherited.
    /// </summary>
    sealed class PerceptronNetworkFactory
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
            var hiddenLayerConfigurations = new LayerConfiguration[0];
            return Create(inputLayerConfiguration, hiddenLayerConfigurations, outputLayerConfiguration);
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
            throw new NotImplementedException();
        }
    }
}
