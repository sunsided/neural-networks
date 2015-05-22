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
        public readonly uint NeuronCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerConfiguration"/> class.
        /// </summary>
        /// <param name="neuronCount">The neuron count.</param>
        public LayerConfiguration(uint neuronCount)
        {
            NeuronCount = neuronCount;
        }
    }
}
