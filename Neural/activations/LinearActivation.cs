namespace Neural.Activations
{
    /// <summary>
    /// Linear activation function.
    /// </summary>
    sealed class LinearActivation : IActivation
    {
        /// <summary>
        /// Calculates the activation given the perceptron's sum of weighted inputs.
        /// </summary>
        /// <param name="sum">The sum of weighted inputs.</param>
        /// <returns>System.Single.</returns>
        public float Activate(float sum)
        {
            return sum;
        }

        /// <summary>
        /// Calculates the derivative of the activation <paramref name="value" />.
        /// </summary>
        /// <param name="value">The sum of weighted inputs.</param>
        /// <returns>System.Single.</returns>
        public float Derivative(float value)
        {
            return 1;
        }
    }
}
