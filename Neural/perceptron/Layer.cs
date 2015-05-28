using System;
using System.Diagnostics;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using Neural.Activations;

namespace Neural.Perceptron
{
    /// <summary>
    /// A perceptron layer.
    /// </summary>
    [DebuggerDisplay("{Type,nq}: {_weightMatrix.ColumnCount,nq} --> {_weightMatrix.RowCount,nq}")]
    sealed class Layer
    {
        /// <summary>
        /// The weight vector of the bias units
        /// </summary>
        /// <seealso cref="_weightMatrix"/>
        [NotNull]
        private readonly Vector<float> _biasVector;

        /// <summary>
        /// The weight matrix (Theta)
        /// </summary>
        /// <seealso cref="_biasVector"/>
        [NotNull]
        private readonly Matrix<float> _weightMatrix;

        /// <summary>
        /// The transfer function
        /// </summary>
        /// <seealso cref="_gradientFunction"/>
        [NotNull]
        private readonly Func<Vector<float>, Vector<float>> _transferFunction;

        /// <summary>
        /// The gradient function
        /// </summary>
        /// <seealso cref="_transferFunction"/>
        [NotNull]
        private readonly Func<Vector<float>, Vector<float>> _gradientFunction;

        /// <summary>
        /// The input layer
        /// </summary>
        /// <seealso cref="Previous"/>
        [CanBeNull]
        private readonly Layer _previousLayer;

        /// <summary>
        /// The input layer
        /// </summary>
        /// <seealso cref="Next"/>
        [NotNull]
        private readonly WeakReference<Layer> _nextLayer;

        /// <summary>
        /// Gets the input layer.
        /// </summary>
        /// <value>The input layer.</value>
        /// <seealso cref="Next"/>
        public Layer Previous
        {
            [CanBeNull]
            get {  return _previousLayer; }
        }

        /// <summary>
        /// Gets the output layer.
        /// </summary>
        /// <value>The output layer.</value>
        /// <seealso cref="Previous"/>
        public Layer Next
        {
            [CanBeNull]
            get
            {
                Layer output;
                return _nextLayer.TryGetTarget(out output) ? output : null;
            }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public LayerType Type
        {
            [Pure]
            get
            {
                if (Previous == null) return LayerType.Input;
                return (Next == null)
                    ? LayerType.Output
                    : LayerType.Hidden;
            }
        }

        /// <summary>
        /// Gets the number of neurons in this layer.
        /// </summary>
        /// <value>The number of neurons.</value>
        public int NeuronCount
        {
            [Pure]
            get { return _weightMatrix.RowCount; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Layer" /> class.
        /// </summary>
        /// <param name="previousLayer">The previous layer.</param>
        /// <param name="nextLayer">The next layer.</param>
        /// <param name="biasVector">The bias vector.</param>
        /// <param name="weightMatrix">The weight matrix.</param>
        /// <param name="transferFunction">The activation function.</param>
        public Layer([CanBeNull] Layer previousLayer, [NotNull] WeakReference<Layer> nextLayer, [NotNull] Vector<float> biasVector, [NotNull] Matrix<float> weightMatrix, [NotNull] ITransfer transferFunction)
        {
            _previousLayer = previousLayer;
            _nextLayer = nextLayer;
            _biasVector = biasVector;
            _weightMatrix = weightMatrix;
            _transferFunction = transferFunction.Transfer;
            _gradientFunction = transferFunction.Derivative;
        }

        /// <summary>
        /// Performs a feed-forward step of the layer's <paramref name="input"/>.
        /// </summary>
        /// <param name="input">The row vector of inputs.</param>
        /// <returns>The activations of this layer's perceptrons.</returns>
        [Pure]
        public FeedforwardResult Feedforward([NotNull] Vector<float> input)
        {
            var matrix = _weightMatrix;
            var transferActivation = _transferFunction;

            // calculate activations for each neuron in the layer
            var activation = matrix * input + _biasVector;

            // apply the activation function to each weighted activation
            var output = transferActivation(activation);

            return new FeedforwardResult(this, activation, output);
        }

        /// <summary>
        /// Performs a backpropagation step of the layer's <paramref name="errors" />.
        /// </summary>
        /// <param name="errors">The training errors.</param>
        /// <param name="weightedInputActivations">This layer's weighted input activations.</param>
        /// <returns>The activations of this layer's perceptrons.</returns>
        [Pure, NotNull]
        public BackpropagationResult Backpropagate([NotNull] Vector<float> errors, Vector<float> weightedInputActivations)
        {
            // calculate the gradient of the activation function
            var calculateGradient = _gradientFunction;
            var gradient = calculateGradient(weightedInputActivations);

            // calculate the weighting error for the current layer
            var matrix = _weightMatrix.Transpose();
            var weightingErrors = (matrix * errors).PointwiseMultiply(gradient);

            // calculate the bias error for the current layer
            var biasError = _biasVector * errors;

            return new BackpropagationResult(weightingErrors, biasError);
        }
    }
}
