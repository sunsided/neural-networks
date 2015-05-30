using System.Collections.Generic;
using JetBrains.Annotations;
using Neural.Perceptron;

namespace Neural.Cost
{
    internal interface ICostGradientFunction
    {
        /// <summary>
        /// Calculates the cost and the gradient of the cost function given the training examples.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="lambda">The regularization parameter.</param>
        /// <returns>System.Single.</returns>
        [Pure]
        TrainingResult CalculateCostAndGradient([NotNull] Network network, [NotNull] IReadOnlyCollection<TrainingExample> trainingSet, float lambda);
    }
}