using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace Widemeadows.MachineLearning.Neural.Training.Backpropagation
{
    /// <summary>
    /// Struct BackpropagationResult
    /// </summary>
    public struct BackpropagationResult
    {
        /// <summary>
        /// The weighting errors
        /// </summary>
        [NotNull]
        public readonly Vector<float> WeightErrors;

        /// <summary>
        /// The bias error
        /// </summary>
        public readonly float BiasError;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackpropagationResult"/> struct.
        /// </summary>
        /// <param name="weightErrors">The weighting errors.</param>
        /// <param name="biasError">The bias error.</param>
        public BackpropagationResult([NotNull] Vector<float> weightErrors, float biasError)
        {
            WeightErrors = weightErrors;
            BiasError = biasError;
        }
    }
}
