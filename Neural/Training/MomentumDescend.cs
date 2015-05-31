using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using Neural.Cost;
using Neural.Perceptron;

namespace Neural.Training
{
    /// <summary>
    /// Momentum-based gradient descend
    /// </summary>
    sealed class MomentumDescend : ITraining
    {
        private readonly ICostGradient _cost;
        private float _learningRate = DefaultLearningRate;
        private float _momentum = DefaultMomentum;
        private int _maximumIterationCount = DefaultMaximumIterationCount;
        private int _minimumIterationCount = DefaultMinimumIterationCount;
        private float _costEpsilon = DefaultEpsilon;
        private float _regularizationStrength = DefaultLambda;

        /// <summary>
        /// Initializes a new instance of the <see cref="MomentumDescend"/> class.
        /// </summary>
        /// <param name="cost">The cost function.</param>
        public MomentumDescend([NotNull] ICostFunction cost)
        {
            _cost = new DefaultCostGradient(cost);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MomentumDescend"/> class.
        /// </summary>
        /// <param name="cost">The cost function.</param>
        public MomentumDescend([NotNull] ICostGradient cost)
        {
            _cost = cost;
        }

        /// <summary>
        /// The default learning rate
        /// </summary>
        private const float DefaultLearningRate = 0.05F;

        /// <summary>
        /// The default momentum
        /// </summary>
        private const float DefaultMomentum = 0.8F;

        /// <summary>
        /// The default maximum iteration count
        /// </summary>
        private const int DefaultMaximumIterationCount = 1000;

        /// <summary>
        /// The default minimum iteration count
        /// </summary>
        private const int DefaultMinimumIterationCount = DefaultMaximumIterationCount/10;

        /// <summary>
        /// The default epsilon
        /// </summary>
        private const float DefaultEpsilon = 5E-6F;

        /// <summary>
        /// The default epsilon
        /// </summary>
        private const float DefaultLambda = 0.01F;

        /// <summary>
        /// Gets or sets the learning rate.
        /// </summary>
        /// <value>The learning rate.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Value must be nonnegative</exception>
        /// <exception cref="System.NotFiniteNumberException">Value must be a finite number</exception>
        public float LearningRate
        {
            get { return _learningRate; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value", value, "Value must be nonnegative");
                if (double.IsInfinity(value) || double.IsNaN(value)) throw new NotFiniteNumberException("Value must be a finite number", value);
                _learningRate = value;
            }
        }

        /// <summary>
        /// Gets or sets the descend momentum.
        /// </summary>
        /// <value>The momentum.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Value must be nonnegative</exception>
        /// <exception cref="System.NotFiniteNumberException">Value must be a finite number</exception>
        public float Momentum
        {
            get { return _momentum; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value", value, "Value must be nonnegative");
                if (double.IsInfinity(value) || double.IsNaN(value)) throw new NotFiniteNumberException("Value must be a finite number", value);
                _momentum = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum iteration count.
        /// </summary>
        /// <value>The maximum iteration count.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Value must be positive</exception>
        public int MaximumIterationCount
        {
            get { return _maximumIterationCount; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("value", value, "Value must be positive");
                _maximumIterationCount = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum iteration count.
        /// </summary>
        /// <value>The maximum iteration count.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Value must be positive</exception>
        public int MinimumIterationCount
        {
            get { return _minimumIterationCount; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("value", value, "Value must be positive");
                _minimumIterationCount = value;
            }
        }

        /// <summary>
        /// Gets or sets the learning cost epsilon value.
        /// <para>
        /// If the learning cost of the trained network changes less than this value,
        /// training will be considered successful.
        /// </para>
        /// </summary>
        /// <value>The cost epsilon.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Value must be nonnegative</exception>
        /// <exception cref="System.NotFiniteNumberException">Value must be a finite number</exception>
        public float CostEpsilon
        {
            get { return _costEpsilon; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value", value, "Value must be nonnegative");
                if (double.IsInfinity(value) || double.IsNaN(value)) throw new NotFiniteNumberException("Value must be a finite number", value);
                _costEpsilon = value;
            }
        }

        /// <summary>
        /// Gets or sets the regularization strength.
        /// </summary>
        /// <value>The regularization strength.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Regularization parameter must be nonnegative</exception>
        /// <exception cref="System.NotFiniteNumberException">Regularization parameter must be a finite number</exception>
        public float RegularizationStrength
        {
            get { return _regularizationStrength; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value", value, "Regularization parameter must be nonnegative");
                if (double.IsInfinity(value) || double.IsNaN(value)) throw new NotFiniteNumberException("Regularization parameter must be a finite number", value);
                _regularizationStrength = value;
            }
        }

        /// <summary>
        /// Trains the specified <paramref name="network"/> using the <paramref name="trainingSet"/>.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="trainingSet">The training set.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Either <paramref name="network"/> or <paramref name="trainingSet"/> was null.
        /// </exception>
        public TrainingStop Train(Network network, IReadOnlyCollection<TrainingExample> trainingSet)
        {
            if (network == null) throw new ArgumentNullException("network", "The network reference must not be null");
            if (trainingSet == null) throw new ArgumentNullException("trainingSet", "The training set must not be null");

            // for momentum-based gradient descent, we need to keep track of the
            // weight delta used in the previous iteration.
            // The input layer is excluded from this, as there is nothing to update.
            var previousDeltas = network.Skip(1).ToDictionary(layer => layer, ErrorGradient.EmptyFromLayer);

            // learning parameters.
            var learningRate = LearningRate;
            var momentum = Momentum;
            var lambda = RegularizationStrength;
            var epsilon = CostEpsilon;
            var minimumIterations = MinimumIterationCount;
            var maximumIterations = MaximumIterationCount;
            var costFunction = _cost;

            var lastCost = 0.0F;
            for (int i = 0; i < maximumIterations; ++i)
            {
                var trainingResult = costFunction.CalculateCostAndGradient(network, trainingSet, lambda);

                // determine cost delta and early-exit if it is smaller than epsilon
                var costDelta = lastCost - trainingResult.Cost;
                if (costDelta <= epsilon && i >= minimumIterations)
                {
                    Debug.WriteLine("Training stopped at iteration {0} because cost delta {1} <= {2}", i, costDelta, epsilon);
                    return TrainingStop.EpsilonReached;
                }

                // store the cost for the next iteration
                lastCost = trainingResult.Cost;
                Debug.WriteLine("iteration {0}: cost {1}, cost delta {2}", i, trainingResult.Cost, costDelta);

                if (float.IsNaN(lastCost))
                {
                    throw new InvalidOperationException("Cost evaluated to Single.NaN");
                }

                // perform a single gradient descend step
                GradientDescend(trainingResult, previousDeltas, learningRate, momentum);
            }

            Debug.WriteLine("Training terminated at cost {0}", lastCost);
            return TrainingStop.MaximumIterationsReached;
        }

        /// <summary>
        /// Updates the layer weights according to the <paramref name="trainingResult" />
        /// </summary>
        /// <param name="trainingResult">The training result.</param>
        /// <param name="previousDeltas">The previous iteration deltas per layer.</param>
        /// <param name="learningRate">The learning rate.</param>
        /// <param name="momentum">The descent momentum.</param>
        private static void GradientDescend(TrainingResult trainingResult, [NotNull] IDictionary<Layer, ErrorGradient> previousDeltas, float learningRate, float momentum)
        {
            var gradientEntries = trainingResult.ErrorGradients;
            // TODO: Since each gradient describes a single layer, this operation is fully data parallel
            foreach (var entry in gradientEntries)
            {
                var layer = entry.Key;
                var gradient = entry.Value;
                var previousDelta = previousDeltas[layer];

                // calculate the descend step size and update
                // the layer's weights accordingly
                var weightDelta = learningRate * gradient.Weight + momentum * previousDelta.Weight;
                var biasDelta = learningRate * gradient.Bias + momentum * previousDelta.Bias;

                // note that simply by definition of the error's sign we subtract the
                // deltas from the weights instead of adding them.
                layer.Weights.MapIndexedInplace((row, column, value) => value - weightDelta[row, column]);
                layer.Bias.MapIndexedInplace((row, value) => value - biasDelta[row]);

                // store the current delta
                previousDeltas[layer] = new ErrorGradient(weightDelta, biasDelta);
            }
        }
    }
}
