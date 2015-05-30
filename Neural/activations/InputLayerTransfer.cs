using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace Neural.Activations
{
    /// <summary>
    /// Invalid activation function that acts as a placeholder in the
    /// input layer configuration.
    /// <para>
    /// It always acts as an identity transfer function and throws an exception
    /// whenever the gradient is attempted to be obtained.
    /// </para>
    /// </summary>
    sealed class InputLayerTransfer : ITransfer
    {
        /// <summary>
        /// Always return <paramref name="z"/>
        /// </summary>
        /// <param name="z">The value at which to calculate the activation.</param>
        /// <returns>System.Single.</returns>
        public float Transfer(float z)
        {
            return z;
        }

        /// <summary>
        /// Always return <paramref name="z"/>
        /// </summary>
        /// <param name="z">The values at which to calculate the activation.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        [NotNull]
        public Vector<float> Transfer([NotNull] Vector<float> z)
        {
            return z;
        }

        /// <summary>
        /// Always fails with a <see cref="InvalidOperationException" />.
        /// </summary>
        /// <param name="z">The value at which to evaluate the gradients.</param>
        /// <returns>System.Single.</returns>
        /// <exception cref="InvalidOperationException">An activation function of the input layer has been used erroneously.</exception>
        public float Derivative(float z)
        {
            throw new InvalidOperationException("An activation function of the input layer has been used erroneously.");
        }

        /// <summary>
        /// Always fails with a <see cref="InvalidOperationException" />.
        /// </summary>
        /// <param name="z">The value at which to evaluate the gradients.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        /// <exception cref="InvalidOperationException">An activation function of the input layer has been used erroneously.</exception>
        public Vector<float> Derivative(Vector<float> z)
        {
            throw new InvalidOperationException("An activation function of the input layer has been used erroneously.");
        }
    }
}
