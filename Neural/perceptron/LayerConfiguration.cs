using System;
using JetBrains.Annotations;
using Neural.Activations;

namespace Neural.Perceptron
{
    /// <summary>
    /// Configuration of a single perceptron layer.
    /// </summary>
    struct LayerConfiguration
    {
        /// <summary>
        /// Creates a configuration for the input layer.
        /// </summary>
        /// <param name="neuronCount">The neuron count.</param>
        /// <returns>LayerConfiguration.</returns>
        public static LayerConfiguration ForInput(int neuronCount)
        {
            return new LayerConfiguration(neuronCount, new InputLayerTransfer());
        }

        /// <summary>
        /// Creates a configuration for the hidden layer.
        /// </summary>
        /// <param name="neuronCount">The neuron count.</param>
        /// <param name="activation">The activation function.</param>
        /// <returns>LayerConfiguration.</returns>
        public static LayerConfiguration ForHidden(int neuronCount, [NotNull] ITransfer activation)
        {
            return new LayerConfiguration(neuronCount, activation);
        }

        /// <summary>
        /// Creates a configuration for the output layer.
        /// </summary>
        /// <param name="neuronCount">The neuron count.</param>
        /// <param name="activation">The activation function.</param>
        /// <returns>LayerConfiguration.</returns>
        public static LayerConfiguration ForOutput(int neuronCount, [NotNull] ITransfer activation)
        {
            return new LayerConfiguration(neuronCount, activation);
        }

        /// <summary>
        /// Gets the number of neurons in this layer.
        /// </summary>
        /// <value>The neuron count.</value>
        public readonly int NeuronCount;

        /// <summary>
        /// Gets the activation function of all neurons in the layer.
        /// </summary>
        public readonly ITransfer ActivationFunction;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerConfiguration" /> class.
        /// </summary>
        /// <param name="neuronCount">The number of neurons in this layer.</param>
        /// <param name="activationFunction">The activation function.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of neurons was negative or zero.</exception>
        public LayerConfiguration(int neuronCount, [NotNull] ITransfer activationFunction)
        {
            if (neuronCount <= 0) throw new ArgumentOutOfRangeException("neuronCount", neuronCount, "The number of neurons must be positive.");
            
            NeuronCount = neuronCount;
            ActivationFunction = activationFunction;
        }
    }
}
