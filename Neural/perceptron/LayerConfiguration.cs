using System;
using JetBrains.Annotations;

namespace Neural.perceptron
{
    /// <summary>
    /// Configuration of a single perceptron layer.
    /// </summary>
    struct LayerConfiguration
    {
        /// <summary>
        /// Gets the number of neurons in this layer.
        /// </summary>
        /// <value>The neuron count.</value>
        public readonly int NeuronCount;

        /// <summary>
        /// Gets the activation function of all neurons in the layer.
        /// </summary>
        public readonly IActivation ActivationFunction;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerConfiguration" /> class.
        /// </summary>
        /// <param name="neuronCount">The number of neurons in this layer.</param>
        /// <param name="activationFunction">The activation function.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of neurons was negative or zero.</exception>
        public LayerConfiguration(int neuronCount, [NotNull] IActivation activationFunction)
        {
            if (neuronCount <= 0) throw new ArgumentOutOfRangeException("neuronCount", neuronCount, "The number of neurons must be positive.");
            
            NeuronCount = neuronCount;
            ActivationFunction = activationFunction;
        }
    }
}
