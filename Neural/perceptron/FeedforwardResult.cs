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
        /// The layer's activation, i.e. weighted sum of net inputs (z).
        /// </summary>
        [NotNull]
        public readonly Vector<float> Activation;

        /// <summary>
        /// The layer's output, i.e. transferred activation.
        /// </summary>
        [NotNull]
        public readonly Vector<float> Output;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedforwardResult" /> struct.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="activation">The layer's activation.</param>
        /// <param name="output">The layer's output.</param>
        public FeedforwardResult([NotNull] Layer parent, [NotNull] Vector<float> activation, [NotNull] Vector<float> output)
        {
            Layer = parent;
            Activation = activation;
            Output = output;
        }
    }
}
