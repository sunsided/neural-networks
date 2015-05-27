using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace Neural.Activations
{
    /// <summary>
    /// An activation function for a <see cref="Perceptron" />.
    /// </summary>
    interface IActivation
    {
        /// <summary>
        /// Calculates the activation of the value <paramref name="z" />
        /// </summary>
        /// <param name="z">The value at which to calculate the activation.</param>
        /// <returns>System.Single.</returns>
        [Pure]
        float Transfer(float z);

        /// <summary>
        /// Calculates the activation of the value <paramref name="z" />
        /// </summary>
        /// <param name="z">The values at which to calculate the activation.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        [Pure]
        Vector<float> Transfer(Vector<float> z);

        /// <summary>
        /// Calculates the gradient of the activation evaluated at <paramref name="z" />.
        /// </summary>
        /// <param name="z">The value at which to evaluate the gradients.</param>
        /// <returns>System.Single.</returns>
        [Pure]
        float Gradient(float z);

        /// <summary>
        /// Calculates the gradient of the activation evaluated at each value of <paramref name="z" />.
        /// </summary>
        /// <param name="z">The value at which to evaluate the gradients.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        [Pure]
        Vector<float> Gradient(Vector<float> z);
    }
}
