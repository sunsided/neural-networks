using System;

namespace Neural.perceptron
{
    /// <summary>
    /// Activation function that returns the sign of the input.
    /// </summary>
    sealed class SignActivation : IActivation
    {
        /// <summary>
        /// Calculates the activation given the perceptron's sum of weighted inputs.
        /// </summary>
        /// <param name="value">The sum of weighted inputs.</param>
        /// <returns>System.Single.</returns>
        /// <exception cref="ArithmeticException"><paramref name="value" /> is equal to <see cref="F:System.Single.NaN" />. </exception>
        public float Activate(float value)
        {
            return Math.Sign(value);
        }
    }
}
