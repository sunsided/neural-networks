using System;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Widemeadows.MachineLearning.Neural.Perceptron;

namespace Widemeadows.MachineLearning.Neural.Training
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
        /// <param name="progress">The progress.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>TrainingStop.</returns>
        /// <exception cref="System.ArgumentNullException">Either <paramref name="network" /> or <paramref name="trainingSet" /> was null.</exception>
        TrainingStop Train([NotNull] Network network, [NotNull] IReadOnlyCollection<TrainingExample> trainingSet, [CanBeNull] IProgress<TrainingProgress> progress, CancellationToken cancellationToken);
    }
}