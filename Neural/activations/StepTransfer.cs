using System;
using MathNet.Numerics.LinearAlgebra;
using Neural.Cost;

namespace Neural.Activations
{
    /// <summary>
    /// Heaviside step function.
    /// <remarks>
    /// <para>
    /// The step function is nondifferentiable and thus cannot be used in backpropagation.
    /// As a consequence it can only be applied to the last neuron, where it serves as
    /// a binary classifier.
    /// </para>
    /// <para>
    /// Also note that this function won't work with the <see cref="LogisticCost"/> function,
    /// as this takes the logarithm of <c>1-cost</c> and <c>cost</c>, rendering it broken
    /// for either one of the binary output values unless the <see cref="Epsilon"/> property
    /// of this class is set to a small value.
    /// </para>
    /// </remarks>
    /// </summary>
    public sealed class StepTransfer : ITransfer
    {
        /// <summary>
        /// The epsilon value
        /// </summary>
        private float _epsilon = DefaultEpsilon;

        /// <summary>
        /// The epsilon
        /// </summary>
        private const float DefaultEpsilon = 0;

        /// <summary>
        /// Gets or sets the epsilon.
        /// </summary>
        /// <value>The epsilon.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Value must be positive and less than 0.5</exception>
        /// <exception cref="System.NotFiniteNumberException">Value must be finite</exception>
        public float Epsilon
        {
            get { return _epsilon; }
            set
            {
                if (value < 0 || value >= 0.5) throw new ArgumentOutOfRangeException("value", value, "Value must be positive and less than 0.5");
                if (float.IsInfinity(value) || float.IsNaN(value)) throw new NotFiniteNumberException("Value must be finite", value);
                _epsilon = value;
            }
        }

        /// <summary>
        /// Applies the transfer function to the values of <paramref name="z" />.
        /// </summary>
        /// <param name="z">The values at which to calculate the activation.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        public Vector<float> Transfer(Vector<float> z)
        {
            var epsilon = _epsilon;
            return z.Map(value => value > 0 ? 1F - epsilon : epsilon);
        }

        /// <summary>
        /// Calculates the gradient of the transfer function evaluated at each value of <paramref name="z" />.
        /// </summary>
        /// <param name="z">The value at which to evaluate the gradients.</param>
        /// <param name="activations">The original activations obtained in the feedforward pass.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        /// <exception cref="System.InvalidOperationException">Attempted to use the heaviside transfer function in a hidden neuron.</exception>
        public Vector<float> Derivative(Vector<float> z, Vector<float> activations)
        {
            throw new InvalidOperationException("Attempted to use the heaviside transfer function in a hidden neuron.");

            // var epsilon = _epsilon;
            // return z.Map(value => Math.Abs(value) < 0.000001F ? 1F - epsilon : epsilon); // that's just stupid.
        }
    }
}
