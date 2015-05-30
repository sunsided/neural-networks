using System.Collections.Generic;
using JetBrains.Annotations;
using Neural.Perceptron;

namespace Neural.Training
{
    /// <summary>
    /// Interface for network training implementations
    /// </summary>
    public interface ITraining
    {
        /// <summary>
        /// Trains the specified <paramref name="network" /> using the <paramref name="trainingSet" />.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="trainingSet">The training set.</param>
        /// <returns>TrainingStop.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Either <paramref name="network"/> or <paramref name="trainingSet"/> was null.
        /// </exception>
        TrainingStop Train([NotNull] Network network, [NotNull] IReadOnlyCollection<TrainingExample> trainingSet);
    }
}