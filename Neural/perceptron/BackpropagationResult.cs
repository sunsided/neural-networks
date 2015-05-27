using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace Neural.Perceptron
{
    /// <summary>
    /// Struct BackpropagationResult
    /// </summary>
    struct BackpropagationResult
    {
        /// <summary>
        /// The weighting errors
        /// </summary>
        [NotNull]
        public readonly Vector<float> WeightingErrors;

        /// <summary>
        /// The bias error
        /// </summary>
        public readonly float BiasError;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackpropagationResult"/> struct.
        /// </summary>
        /// <param name="weightingErrors">The weighting errors.</param>
        /// <param name="biasError">The bias error.</param>
        public BackpropagationResult([NotNull] Vector<float> weightingErrors, float biasError)
        {
            WeightingErrors = weightingErrors;
            BiasError = biasError;
        }
    }
}
