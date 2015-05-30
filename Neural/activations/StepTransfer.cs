using System;
using MathNet.Numerics.LinearAlgebra;

namespace Neural.Activations
{
    /// <summary>
    /// Heaviside step function.
    /// <remarks>
    /// The step function is nondifferentiable and thus cannot be used in backpropagation.
    /// As a consequence it can only be applied to the last neuron, where it serves as
    /// a binary classifier.
    /// </remarks>
    /// </summary>
    sealed class StepTransfer : ITransfer
    {
        /// <summary>
        /// Applies the transfer function to the values of <paramref name="z" />.
        /// </summary>
        /// <param name="z">The values at which to calculate the activation.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        public Vector<float> Transfer(Vector<float> z)
        {
            return z.Map(value => value >= 0 ? 1F : 0F);
        }

        /// <summary>
        /// Calculates the gradient of the transfer function evaluated at each value of <paramref name="z" />.
        /// </summary>
        /// <param name="z">The value at which to evaluate the gradients.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        /// <exception cref="System.InvalidOperationException">Attempted to use the heaviside transfer function in a hidden neuron.</exception>
        public Vector<float> Derivative(Vector<float> z)
        {
            throw new InvalidOperationException("Attempted to use the heaviside transfer function in a hidden neuron.");
            return z.Map(value => Math.Abs(value) < 0.000001F ? 1F : 0F); // that's just stupid.
        }
    }
}
