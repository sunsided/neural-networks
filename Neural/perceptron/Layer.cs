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
    [DebuggerDisplay("Maps {_weightMatrix.ColumnCount,nq} inputs to {_weightMatrix.RowCount,nq} outputs")]
    sealed class Layer
    {
        /// <summary>
        /// The weight vector of the bias units
        /// </summary>
        [NotNull]
        private readonly Vector<float> _biasVector;

        /// <summary>
        /// The weight matrix (Theta)
        /// </summary>
        [NotNull]
        private readonly Matrix<float> _weightMatrix;

        /// <summary>
        /// The transfer function
        /// </summary>
        [NotNull]
        private readonly Func<Vector<float>, Vector<float>> _transferFunction;

        /// <summary>
        /// The gradient function
        /// </summary>
        [NotNull]
        private readonly Func<Vector<float>, Vector<float>> _gradientFunction;

        /// <summary>
        /// The input layer
        /// </summary>
        [CanBeNull]
        private readonly Layer _inputLayer;

        /// <summary>
        /// Gets the input layer.
        /// </summary>
        /// <value>The input layer.</value>
        public Layer InputLayer
        {
            [CanBeNull]
            get {  return _inputLayer; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is first layer in the network.
        /// </summary>
        /// <value><see langword="true" /> if this instance is first layer; otherwise, <see langword="false" />.</value>
        public bool IsFirstLayer
        {
            get { return _inputLayer == null; }
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
        /// <param name="biasVector"></param>
        /// <param name="weightMatrix">The weight matrix.</param>
        /// <param name="transferFunction">The activation function.</param>
        public Layer([CanBeNull] Layer inputLayer, [NotNull] Vector<float> biasVector, [NotNull] Matrix<float> weightMatrix, [NotNull] ITransfer transferFunction)
        {
            _inputLayer = inputLayer;
            _biasVector = biasVector;
            _weightMatrix = weightMatrix;
            _transferFunction = transferFunction.Transfer;
            _gradientFunction = transferFunction.Gradient;
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
