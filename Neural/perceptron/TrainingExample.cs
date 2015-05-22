using System.Collections.Generic;
using JetBrains.Annotations;

namespace Neural.Perceptron
{
    /// <summary>
    /// Defines a training example
    /// </summary>
    struct TrainingExample
    {
        /// <summary>
        /// The input values
        /// </summary>
        [NotNull]
        public readonly IReadOnlyList<float> Inputs;

        /// <summary>
        /// The according output values
        /// </summary>
        [NotNull]
        public readonly IReadOnlyList<float> Outputs;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrainingExample"/> struct.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        /// <param name="outputs">The outputs.</param>
        public TrainingExample(IReadOnlyList<float> inputs, IReadOnlyList<float> outputs)
        {
            Inputs = inputs;
            Outputs = outputs;
        }
    }
}
