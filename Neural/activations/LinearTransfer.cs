using MathNet.Numerics.LinearAlgebra;

namespace Neural.Activations
{
    /// <summary>
    /// Linear activation function.
    /// </summary>
    sealed class LinearTransfer : ITransfer
    {
        /// <summary>
        /// Calculates the activation of the value <paramref name="z" />
        /// </summary>
        /// <param name="z">The value at which to calculate the activation.</param>
        /// <returns>System.Single.</returns>
        public float Transfer(float z)
        {
            return z;
        }

        /// <summary>
        /// Calculates the activation of the value <paramref name="z" />
        /// </summary>
        /// <param name="z">The values at which to calculate the activation.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        public Vector<float> Transfer(Vector<float> z)
        {
            return z;
        }

        /// <summary>
        /// Calculates the gradient of the activation evaluated at <paramref name="z" />.
        /// </summary>
        /// <param name="z">The value at which to evaluate the gradients.</param>
        /// <returns>System.Single.</returns>
        public float Gradient(float z)
        {
            return 1;
        }

        /// <summary>
        /// Calculates the gradient of the activation evaluated at each value of <paramref name="z" />.
        /// </summary>
        /// <param name="z">The value at which to evaluate the gradients.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        public Vector<float> Gradient(Vector<float> z)
        {
            return z.Map(value => 1F);
        }
    }
}
