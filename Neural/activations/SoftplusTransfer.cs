﻿using System;
using MathNet.Numerics.LinearAlgebra;

namespace Widemeadows.MachineLearning.Neural.Activations
{
    /// <summary>
    /// Softplus transfer function.
    /// </summary>
    /// <remarks>
    /// The softplus function maps its outputs to the range of [0; +inf(
    /// </remarks>
    public sealed class SoftplusTransfer : ITransfer
    {
        /// <summary>
        /// Applies the transfer function to the values of <paramref name="z" />.
        /// </summary>
        /// <param name="z">The values at which to calculate the activation.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        public Vector<float> Transfer(Vector<float> z)
        {
            return z.Map(value => (float)Math.Log(1F + (float)Math.Exp(value)), Zeros.Include);
        }

        /// <summary>
        /// Calculates the gradient of the transfer function evaluated at each value of <paramref name="z" />.
        /// </summary>
        /// <param name="z">The value at which to evaluate the gradients.</param>
        /// <param name="activations">The original activations obtained in the feedforward pass.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        public Vector<float> Derivative(Vector<float> z, Vector<float> activations)
        {
            return z.Map(value => 1.0F / (1F + (float)Math.Exp(-value)));
        }
    }
}
