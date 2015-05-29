﻿using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace Neural.Perceptron
{
    /// <summary>
    /// Struct FeedforwardResult
    /// </summary>
    struct FeedforwardResult
    {
        /// <summary>
        /// The layer this result belongs to.
        /// </summary>
        [NotNull]
        public readonly Layer Layer;

        /// <summary>
        /// The layer's weighted sums of net inputs (Z).
        /// </summary>
        [NotNull]
        public readonly Vector<float> WeightedInputs;

        /// <summary>
        /// The layer's output, i.e. transferred activation (A).
        /// </summary>
        [NotNull]
        public readonly Vector<float> Output;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedforwardResult" /> struct.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="weightedInputs">The weighted inputs.</param>
        /// <param name="output">The layer's output.</param>
        public FeedforwardResult([NotNull] Layer parent, [NotNull] Vector<float> weightedInputs, [NotNull] Vector<float> output)
        {
            Layer = parent;
            WeightedInputs = weightedInputs;
            Output = output;
        }
    }
}
