using System;
using MathNet.Numerics.LinearAlgebra;

namespace Neural.Cost
{
    /// <summary>
    /// Logistic regression-like multivariate cost function.
    /// </summary>
    /// <remarks>
    /// This cost function only works for transfer functions that
    /// have an ]0..1[
    /// </remarks>
    sealed class LogisticCost : CostGradientBase, ICostFunction
    {
        /// <summary>
        /// Calculates the network's training cost.
        /// </summary>
        /// <param name="expectedOutput">The expected output, i.e. ground truth.</param>
        /// <param name="networkOutput">The network output.</param>
        /// <returns>System.Single.</returns>
        public override float CalculateCost(Vector<float> expectedOutput, Vector<float> networkOutput)
        {
            var logOutput = networkOutput.Map(v => (float)Math.Log(v));
            var firstPart = expectedOutput * logOutput;

            var logInvOutput = networkOutput.Map(v => (float)Math.Log(1 - v)); // BUG: this will blow up if the network output is actually 1 (or larger)
            var secondPart = (1 - expectedOutput) * logInvOutput;

            return -firstPart - secondPart;
        }
    }
}
