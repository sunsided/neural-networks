namespace Neural.perceptron
{
    /// <summary>
    /// An activation function for a <see cref="Perceptron"/>.
    /// </summary>
    interface IActivation
    {
        /// <summary>
        /// Calculates the activation given the perceptron's sum of weighted inputs.
        /// </summary>
        /// <param name="value">The sum of weighted inputs.</param>
        /// <returns>System.Single.</returns>
        float Activate(float value);
    }
}
