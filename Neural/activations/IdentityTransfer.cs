using MathNet.Numerics.LinearAlgebra;

namespace Widemeadows.MachineLearning.Neural.Activations
{
    /// <summary>
    /// Identity transfer function.
    /// </summary>
    sealed class IdentityTransfer : ITransfer
    {
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
        /// Calculates the gradient of the transfer function evaluated at each value of <paramref name="z" />.
        /// </summary>
        /// <param name="z">The value at which to evaluate the gradients.</param>
        /// <param name="activations">The original activations obtained in the feedforward pass.</param>
        /// <returns>Always the value <c>1</c></returns>
        public Vector<float> Derivative(Vector<float> z, Vector<float> activations)
        {
            return z.Map(value => 1F);
        }
    }
}
