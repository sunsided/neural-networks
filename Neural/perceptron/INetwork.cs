using System.Collections.Generic;
using JetBrains.Annotations;

namespace Widemeadows.MachineLearning.Neural.Perceptron
{
    /// <summary>
    /// Interface INetwork
    /// </summary>
    public interface INetwork
    {
        /// <summary>
        /// Gets the number of input neurons.
        /// </summary>
        /// <value>The number of input neurons.</value>
        int InputNeuronCount { [Pure] get; }

        /// <summary>
        /// Gets the number of output neurons.
        /// </summary>
        /// <value>The number of input neurons.</value>
        int OutputNeuronCount { [Pure] get; }

        /// <summary>
        /// Calculates the outputs given the specified <paramref name="inputs"/>.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        [NotNull]
        IReadOnlyList<float> Evaluate([NotNull] IReadOnlyList<float> inputs);
    }
}