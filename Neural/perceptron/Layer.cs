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
        /// The activation function
        /// </summary>
        [NotNull]
        private readonly Func<Vector<float>, Vector<float>> _activationFunction;

        /// <summary>
        /// The gradient function
        /// </summary>
        [NotNull]
        private readonly Func<Vector<float>, Vector<float>> _gradient;

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
        /// <param name="activationFunction">The activation function.</param>
        public Layer([NotNull]  Vector<float> biasVector, [NotNull] Matrix<float> weightMatrix, [NotNull] IActivation activationFunction)
        {
            _biasVector = biasVector;
            _weightMatrix = weightMatrix;
            _activationFunction = activationFunction.Transfer;
            _gradient = activationFunction.Gradient;
        }

        /// <summary>
        /// Performs a feed-forward step of the layer's <paramref name="activations"/>.
        /// </summary>
        /// <param name="activations">The row vector of activations.</param>
        /// <returns>The activations of this layer's perceptrons.</returns>
        [Pure]
        public FeedforwardResult Feedforward([NotNull] Vector<float> activations)
        {
            var matrix = _weightMatrix;
            var activationFunction = _activationFunction;

            // calculate the sum of weighted activations for each neuron in the layer
            var weightedActivations = matrix * activations + _biasVector;

            // apply the activation function to each weighted activation
            var outputActivations = activationFunction(weightedActivations);

            return new FeedforwardResult(weightedActivations, outputActivations);
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
            var gradientFunction = _gradient;
            var gradient = gradientFunction(weightedInputActivations);

            // calculate the weighting error for the current layer
            var matrix = _weightMatrix.Transpose();
            var weightingErrors = (matrix * errors).PointwiseMultiply(gradient);

            // calculate the bias error for the current layer
            var biasError = _biasVector * errors;

            return new BackpropagationResult(weightingErrors, biasError);
        }
    }
}
