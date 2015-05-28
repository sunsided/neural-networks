using JetBrains.Annotations;
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
        /// The weighted sum of net inputs (z).
        /// </summary>
        [NotNull]
        public readonly Vector<float> Input;

        /// <summary>
        /// The activation value
        /// </summary>
        [NotNull]
        public readonly Vector<float> Activation;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedforwardResult" /> struct.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="input">The z.</param>
        /// <param name="activation">The activation.</param>
        public FeedforwardResult([NotNull] Layer parent, [NotNull] Vector<float> input, [NotNull] Vector<float> activation)
        {
            Layer = parent;
            Input = input;
            Activation = activation;
        }
    }
}
