using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Neural.Perceptron
{
    /// <summary>
    /// Configuration of the perceptron network.
    /// </summary>
    [DebuggerDisplay("{InputLayer.NeuronCount,nq} -> {HiddenLayers.Count,nq}xN -> {OutputLayer.NeuronCount,nq}")]
    struct NetworkConfiguration
    {
        /// <summary>
        /// Gets the input layer configuration.
        /// </summary>
        /// <value>The input layer.</value>
        public readonly LayerConfiguration InputLayer;

        /// <summary>
        /// Gets the output layer configuration.
        /// </summary>
        /// <value>The output layer.</value>
        public readonly LayerConfiguration OutputLayer;

        /// <summary>
        /// Gets the hidden layer configurations.
        /// </summary>
        /// <value>The hidden layer configurations.</value>
        [NotNull]
        public readonly IReadOnlyList<LayerConfiguration> HiddenLayers;

        /// <summary>
        /// Initializes a <see cref="NetworkConfiguration"/> of a network that contains no hidden layers.
        /// </summary>
        /// <param name="inputLayer">The input layer.</param>
        /// <param name="outputLayer">The output layer.</param>
        public NetworkConfiguration(LayerConfiguration inputLayer, LayerConfiguration outputLayer)
            : this(inputLayer, new LayerConfiguration[0], outputLayer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkConfiguration"/> struct.
        /// </summary>
        /// <param name="inputLayer">The input layer.</param>
        /// <param name="hiddenLayers">The hidden layers.</param>
        /// <param name="outputLayer">The output layer.</param>
        public NetworkConfiguration(LayerConfiguration inputLayer, [NotNull] IReadOnlyList<LayerConfiguration> hiddenLayers, LayerConfiguration outputLayer)
        {
            InputLayer = inputLayer;
            OutputLayer = outputLayer;
            HiddenLayers = hiddenLayers;
        }
    }
}
