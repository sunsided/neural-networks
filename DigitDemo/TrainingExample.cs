using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace Widemeadows.MachineLearning.Neural.Demonstration.Digit
{
    /// <summary>
    /// Struct TrainingExample
    /// </summary>
    struct TrainingExample
    {
        /// <summary>
        /// The label (0..9)
        /// </summary>
        public readonly int Label;

        /// <summary>
        /// The pixels
        /// </summary>
        [NotNull]
        public readonly Matrix<float> Pixels;

        /// <summary>
        /// The minimum pixel value
        /// </summary>
        public readonly float Min;

        /// <summary>
        /// The maximum pixel value
        /// </summary>
        public readonly float Max;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrainingExample"/> struct.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="pixels">The pixels.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        public TrainingExample(int label, [NotNull] Matrix<float> pixels, float min, float max)
        {
            Label = label;
            Pixels = pixels;
            Min = min;
            Max = max;
        }
    }
}
