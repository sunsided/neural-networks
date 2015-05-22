using System;

namespace Neural.Activations
{
    /// <summary>
    /// Invalid activation function that acts as a placeholder in the
    /// input layer configuration.
    /// </summary>
    sealed class InputLayerActivation : IActivation
    {
        /// <summary>
        /// Always fails with a <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="value">The sum of weighted inputs.</param>
        /// <returns>System.Single.</returns>
        /// <exception cref="InvalidOperationException">An activation function of the input layer has been used erroneously.</exception>
        public float Activate(float value)
        {
            throw new InvalidOperationException("An activation function of the input layer has been used erroneously.");
        }

        /// <summary>
        /// Always fails with a <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="value">The sum of weighted inputs.</param>
        /// <exception cref="InvalidOperationException">An activation function of the input layer has been used erroneously.</exception>
        public float Derivative(float value)
        {
            throw new InvalidOperationException("An activation function of the input layer has been used erroneously.");            
        }
    }
}
