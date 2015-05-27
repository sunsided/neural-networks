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
        /// Initializes a new instance of the <see cref="FeedforwardResult"/> struct.
        /// </summary>
        /// <param name="input">The z.</param>
        /// <param name="activation">The activation.</param>
        public FeedforwardResult([NotNull] Vector<float> input, [NotNull] Vector<float> activation)
        {
            Input = input;
            Activation = activation;
        }
    }
}
