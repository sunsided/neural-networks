using System;
using System.Diagnostics;

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
        /// Initializes a new instance of the <see cref="LayerConfiguration"/> class.
        /// </summary>
        /// <param name="neuronCount">The number of neurons in this layer.</param>
        /// <exception cref="ArgumentOutOfRangeException">The number of neurons was negative or zero.</exception>
        public LayerConfiguration(int neuronCount)
        {
            if (neuronCount <= 0) throw new ArgumentOutOfRangeException("neuronCount", neuronCount, "The number of neurons must be positive.");
            NeuronCount = neuronCount;
        }
    }
}
