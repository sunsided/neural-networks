using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using Neural.Perceptron;

namespace Neural.Cost
{
    /// <summary>
    /// Logistic regression-like multivariate cost function.
    /// </summary>
    sealed class LogisticCost : CostBase
    {
        /// <summary>
        /// Calculates the network's training cost.
        /// </summary>
        /// <param name="expectedOutput">The expected output, i.e. ground truth.</param>
        /// <param name="networkOutput">The network output.</param>
        /// <returns>System.Single.</returns>
        protected override float CalculateCost(Vector<float> expectedOutput, Vector<float> networkOutput)
        {
            var logOutput = networkOutput.Map(v => (float)Math.Log(v));
            var firstPart = expectedOutput * logOutput;

            var logInvOutput = networkOutput.Map(v => (float)Math.Log(1 - v)); // BUG: this will blow up if the network output is actually 1 (or larger)
            var secondPart = (1 - expectedOutput) * logInvOutput;

            return -firstPart - secondPart;
        }
    }
}
