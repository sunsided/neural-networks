﻿using System;
using MathNet.Numerics.LinearAlgebra;

namespace Neural.Activations
{
    /// <summary>
    /// tanh(x) function.
    /// </summary>
    sealed class TanhTransfer : ITransfer
    {
        /// <summary>
        /// Applies the transfer function to the values of <paramref name="z" />.
        /// </summary>
        /// <param name="z">The values at which to calculate the activation.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        public Vector<float> Transfer(Vector<float> z)
        {
            return z.Map(value => (float)Math.Tanh(value));
        }

        /// <summary>
        /// Calculates the gradient of the transfer function evaluated at each value of <paramref name="z" />.
        /// </summary>
        /// <param name="z">The value at which to evaluate the gradients.</param>
        /// <param name="activations">The original activations obtained in the feedforward pass.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        public Vector<float> Derivative(Vector<float> z, Vector<float> activations)
        {
            // derivative tanh'(x) = 1 - tanh²(x)
            return activations.Map(value => 1F - (value * value));
        }
    }
}