using System.Collections.Generic;
using JetBrains.Annotations;
using Widemeadows.MachineLearning.Neural.Perceptron;

namespace Widemeadows.MachineLearning.Neural.Cost
{
    /// <summary>
    /// Interface ICostGradientFunction
    /// </summary>
    public interface ICostGradient : ICostFunction
    {
        /// <summary>
        /// Calculates the cost and the gradient of the cost function given the training examples.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="lambda">The regularization parameter.</param>
        /// <returns>System.Single.</returns>
        [Pure]
        CostGradient CalculateCostAndGradient([NotNull] Network network, [NotNull] IReadOnlyCollection<TrainingExample> trainingSet, float lambda);
    }
}