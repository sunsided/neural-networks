using System.Collections.Generic;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace Neural.Perceptron
{
    /// <summary>
    /// Defines a training example
    /// </summary>
    public struct TrainingExample
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

        /// <summary>
        /// Initializes a new instance of the <see cref="TrainingExample"/> struct.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        /// <param name="outputs">The outputs.</param>
        internal TrainingExample(Vector<float> inputs, Vector<float> outputs)
            : this(inputs.ToArray(), outputs.ToArray())
        {
        }

        /// <summary>
        /// Obtains the inputs as a <see cref="Vector{T}"/>
        /// </summary>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        [NotNull, Pure]
        internal Vector<float> GetInputs()
        {
            return Vector<float>.Build.SparseOfEnumerable(Inputs);
        }

        /// <summary>
        /// Obtains the outputs as a <see cref="Vector{T}"/>
        /// </summary>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        [NotNull, Pure]
        internal Vector<float> GetOutputs()
        {
            return Vector<float>.Build.SparseOfEnumerable(Outputs);
        }
    }
}
