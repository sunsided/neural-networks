using MathNet.Numerics.LinearAlgebra;

namespace Neural.Cost
{
    /// <summary>
    /// Sum of squared error cost function.
    /// </summary>
    public sealed class SumSquaredErrorCost : ICostFunction
    {
        /// <summary>
        /// Calculates the network's training cost.
        /// </summary>
        /// <param name="expectedOutput">The expected output, i.e. ground truth.</param>
        /// <param name="networkOutput">The network output.</param>
        /// <returns>System.Single.</returns>
        public float CalculateCost(Vector<float> expectedOutput, Vector<float> networkOutput)
        {
            return 0.5F * (expectedOutput - networkOutput).Map(value => value * value).Sum();
        }
    }
}
