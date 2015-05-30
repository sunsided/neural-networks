using System.Collections.Generic;
using JetBrains.Annotations;
using Neural.Perceptron;

namespace Neural.Training
{
    /// <summary>
    /// Momentum-based gradient descend
    /// </summary>
    sealed class MomentumDescend
    {
        /// <summary>
        /// The default learning rate
        /// </summary>
        public const float DefaultLearningRate = 0.05F;

        /// <summary>
        /// The default momentum
        /// </summary>
        public const float DefaultMomentum = 0.8F;

        /// <summary>
        /// The default maximum iteration count
        /// </summary>
        public const int DefaultMaximumIterationCount = 1000;

        /// <summary>
        /// The default minimum iteration count
        /// </summary>
        public const int DefaultMinimumIterationCount = DefaultMaximumIterationCount;

        /// <summary>
        /// The default epsilon
        /// </summary>
        public const float DefaultEpsilon = 5E-6F;

        /// <summary>
        /// Gets the learning rate.
        /// </summary>
        /// <value>The learning rate.</value>
        public float LearningRate { get; private set; }

        /// <summary>
        /// Gets the descend momentum.
        /// </summary>
        /// <value>The momentum.</value>
        public float Momentum { get; private set; }

        /// <summary>
        /// Gets the maximum iteration count.
        /// </summary>
        /// <value>The maximum iteration count.</value>
        public int MaximumIterationCount { get; private set; }

        /// <summary>
        /// Gets the minimum iteration count.
        /// </summary>
        /// <value>The maximum iteration count.</value>
        public int MinimumIterationCount { get; private set; }

        /// <summary>
        /// Gets the learning cost epsilon value.
        /// <para>
        /// If the learning cost of the trained network changes less than this value,
        /// training will be considered successful.
        /// </para>
        /// </summary>
        /// <value>The cost epsilon.</value>
        public float CostEpsilon { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MomentumDescend"/> class.
        /// </summary>
        /// <param name="learningRate">The learning rate.</param>
        /// <param name="momentum">The momentum.</param>
        /// <param name="maximumIterationCount">The maximum iteration count.</param>
        /// <param name="minimumIterationCount">The minimum iteration count.</param>
        /// <param name="costEpsilon">The cost epsilon.</param>
        public MomentumDescend(float learningRate, float momentum, int maximumIterationCount, float costEpsilon = DefaultEpsilon)
            : this(learningRate, momentum, maximumIterationCount, maximumIterationCount, costEpsilon)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MomentumDescend"/> class.
        /// </summary>
        /// <param name="learningRate">The learning rate.</param>
        /// <param name="momentum">The momentum.</param>
        /// <param name="maximumIterationCount">The maximum iteration count.</param>
        /// <param name="minimumIterationCount">The minimum iteration count.</param>
        /// <param name="costEpsilon">The cost epsilon.</param>
        public MomentumDescend(float learningRate, int maximumIterationCount, float costEpsilon = DefaultEpsilon)
            : this(learningRate, DefaultMomentum, maximumIterationCount, maximumIterationCount, costEpsilon)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MomentumDescend"/> class.
        /// </summary>
        /// <param name="learningRate">The learning rate.</param>
        /// <param name="momentum">The momentum.</param>
        /// <param name="maximumIterationCount">The maximum iteration count.</param>
        /// <param name="minimumIterationCount">The minimum iteration count.</param>
        /// <param name="costEpsilon">The cost epsilon.</param>
        public MomentumDescend(float learningRate, float momentum = DefaultMomentum, int maximumIterationCount = DefaultMaximumIterationCount, int minimumIterationCount = DefaultMinimumIterationCount, float costEpsilon = DefaultEpsilon)
        {
            LearningRate = learningRate;
            Momentum = momentum;
            MaximumIterationCount = maximumIterationCount;
            MinimumIterationCount = minimumIterationCount;
            CostEpsilon = costEpsilon;
        }

        /// <summary>
        /// Trains the specified <paramref name="network"/> using the <paramref name="trainingSet"/>.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="network">The network.</param>
        public void Train([NotNull] IReadOnlyCollection<TrainingExample> trainingSet, [NotNull] Network network)
        {

        }
    }
}
