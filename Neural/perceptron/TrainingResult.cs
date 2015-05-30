using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Neural.Perceptron
{
    /// <summary>
    /// Struct TrainingResult
    /// </summary>
    [DebuggerDisplay("cost: {Cost,nq}")]
    struct TrainingResult
    {
        /// <summary>
        /// The training cost for evaluation in gradient descent.
        /// </summary>
        public readonly float Cost;

        /// <summary>
        /// The error gradients per layer.
        /// </summary>
        [NotNull]
        public readonly IReadOnlyDictionary<Layer, ErrorGradient> ErrorGradients;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrainingResult"/> struct.
        /// </summary>
        /// <param name="errorGradients">The error gradients.</param>
        /// <param name="cost">The cost.</param>
        public TrainingResult(float cost, [NotNull] IReadOnlyDictionary<Layer, ErrorGradient> errorGradients)
        {
            Cost = cost;
            ErrorGradients = errorGradients;
        }
    }
}
