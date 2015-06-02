using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using Widemeadows.MachineLearning.Neural.Perceptron;

namespace Widemeadows.MachineLearning.Neural.Cost
{
    /// <summary>
    /// Struct CostGradient
    /// </summary>
    [DebuggerDisplay("cost: {Cost,nq}")]
    public struct CostGradient
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
        /// Initializes a new instance of the <see cref="CostGradient" /> struct.
        /// </summary>
        /// <param name="cost">The network cost.</param>
        /// <param name="errorGradients">The error gradients.</param>
        public CostGradient(float cost, [NotNull] IReadOnlyDictionary<Layer, ErrorGradient> errorGradients)
        {
            Cost = cost;
            ErrorGradients = errorGradients;
        }
    }
}
