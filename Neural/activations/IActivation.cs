using JetBrains.Annotations;

namespace Neural.Activations
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
        [Pure]
        float Activate(float value);

        /// <summary>
        /// Calculates the derivative of the activation <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The sum of weighted inputs.</param>
        /// <returns>System.Single.</returns>
        [Pure]
        float Derivative(float value);
    }
}
