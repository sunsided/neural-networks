using System;
using MathNet.Numerics.LinearAlgebra;

namespace Neural.Activations
{
    /// <summary>
    /// Invalid activation function that acts as a placeholder in the
    /// input layer configuration.
    /// </summary>
    sealed class InputLayerTransfer : ITransfer
    {
        /// <summary>
        /// Always fails with a <see cref="InvalidOperationException" />.
        /// </summary>
        /// <param name="z">The value at which to calculate the activation.</param>
        /// <returns>System.Single.</returns>
        /// <exception cref="InvalidOperationException">An activation function of the input layer has been used erroneously.</exception>
        public float Transfer(float z)
        {
            throw new InvalidOperationException("An activation function of the input layer has been used erroneously.");
        }

        /// <summary>
        /// Always fails with a <see cref="InvalidOperationException" />.
        /// </summary>
        /// <param name="z">The values at which to calculate the activation.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        /// <exception cref="InvalidOperationException">An activation function of the input layer has been used erroneously.</exception>
        public Vector<float> Transfer(Vector<float> z)
        {
            throw new InvalidOperationException("An activation function of the input layer has been used erroneously.");
        }

        /// <summary>
        /// Always fails with a <see cref="InvalidOperationException" />.
        /// </summary>
        /// <param name="z">The value at which to evaluate the gradients.</param>
        /// <returns>System.Single.</returns>
        /// <exception cref="InvalidOperationException">An activation function of the input layer has been used erroneously.</exception>
        public float Gradient(float z)
        {
            throw new InvalidOperationException("An activation function of the input layer has been used erroneously.");
        }

        /// <summary>
        /// Always fails with a <see cref="InvalidOperationException" />.
        /// </summary>
        /// <param name="z">The value at which to evaluate the gradients.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        /// <exception cref="InvalidOperationException">An activation function of the input layer has been used erroneously.</exception>
        public Vector<float> Gradient(Vector<float> z)
        {
            throw new InvalidOperationException("An activation function of the input layer has been used erroneously.");
        }
    }
}
