using MathNet.Numerics.LinearAlgebra;

namespace Neural.Activations
{
    /// <summary>
    /// Identity transfer function.
    /// </summary>
    sealed class IdentityTransfer : ITransfer
    {
        /// <summary>
        /// Applies the transfer function to the value <paramref name="z" />.
        /// </summary>
        /// <param name="z">The value at which to calculate the activation.</param>
        /// <returns>The value of <paramref name="z"/></returns>
        public float Transfer(float z)
        {
            return z;
        }

        /// <summary>
        /// Applies the transfer function to the values of <paramref name="z" />.
        /// </summary>
        /// <param name="z">The values at which to calculate the activation.</param>
        /// <returns>The value of <paramref name="z"/></returns>
        public Vector<float> Transfer(Vector<float> z)
        {
            return z;
        }

        /// <summary>
        /// Calculates the gradient of the transfer function evaluated at <paramref name="z" />.
        /// </summary>
        /// <param name="z">The value at which to evaluate the gradients.</param>
        /// <returns>Always the value <c>1</c></returns>
        public float Derivative(float z)
        {
            return 1;
        }

        /// <summary>
        /// Calculates the gradient of the transfer function evaluated at each value of <paramref name="z" />.
        /// </summary>
        /// <param name="z">The value at which to evaluate the gradients.</param>
        /// <returns>Always the value <c>1</c></returns>
        public Vector<float> Derivative(Vector<float> z)
        {
            return z.Map(value => 1F);
        }
    }
}
