﻿using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace Neural.Activations
{
    /// <summary>
    /// Interface to a differentiable transfer function.
    /// </summary>
    interface ITransfer
    {
        /// <summary>
        /// Applies the transfer function to the value <paramref name="z" />.
        /// </summary>
        /// <param name="z">The value at which to calculate the activation.</param>
        /// <returns>System.Single.</returns>
        [Pure]
        float Transfer(float z);

        /// <summary>
        /// Applies the transfer function to the values of <paramref name="z" />.
        /// </summary>
        /// <param name="z">The values at which to calculate the activation.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        [Pure, NotNull]
        Vector<float> Transfer([NotNull] Vector<float> z);

        /// <summary>
        /// Calculates the gradient of the transfer function evaluated at <paramref name="z" />.
        /// </summary>
        /// <param name="z">The value at which to evaluate the gradients.</param>
        /// <returns>System.Single.</returns>
        [Pure]
        float Derivative(float z);

        /// <summary>
        /// Calculates the gradient of the transfer function evaluated at each value of <paramref name="z" />.
        /// </summary>
        /// <param name="z">The value at which to evaluate the gradients.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        [Pure, NotNull]
        Vector<float> Derivative([NotNull] Vector<float> z);
    }
}
