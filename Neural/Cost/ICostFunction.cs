using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace Widemeadows.MachineLearning.Neural.Cost
{
    /// <summary>
    /// Interface ICostFunction
    /// </summary>
    public interface ICostFunction
    {
        /// <summary>
        /// Calculates the network's training cost.
        /// </summary>
        /// <param name="expectedOutput">The expected output, i.e. ground truth.</param>
        /// <param name="networkOutput">The network output.</param>
        /// <returns>System.Single.</returns>
        float CalculateCost([NotNull] Vector<float> expectedOutput, [NotNull] Vector<float> networkOutput);
    }
}