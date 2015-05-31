using System;
using System.Diagnostics;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using Widemeadows.MachineLearning.Neural.Activations;

namespace Widemeadows.MachineLearning.Neural.Perceptron
{
    /// <summary>
    /// A perceptron layer.
    /// </summary>
    [DebuggerDisplay("{Type,nq}: {_weightMatrix.ColumnCount,nq} --> {_weightMatrix.RowCount,nq}")]
    public sealed class Layer
    {
        /// <summary>
        /// The weight vector of the bias units
        /// </summary>
        /// <seealso cref="_weightMatrix"/>
        [NotNull] private readonly Vector<float> _biasVector;

        /// <summary>
        /// The weight matrix (Theta)
        /// </summary>
        /// <seealso cref="_biasVector"/>
        [NotNull] private readonly Matrix<float> _weightMatrix;

        /// <summary>
        /// The transfer function
        /// </summary>
        /// <seealso cref="_derivativeFunction"/>
        [NotNull] private readonly Func<Vector<float>, Vector<float>> _transferFunction;

        /// <summary>
        /// The gradient function
        /// </summary>
        /// <seealso cref="_transferFunction"/>
        [NotNull] private readonly Func<Vector<float>, Vector<float>, Vector<float>> _derivativeFunction;

        /// <summary>
        /// The input layer
        /// </summary>
        /// <seealso cref="Previous"/>
        [CanBeNull] private readonly Layer _previousLayer;

        /// <summary>
        /// The input layer
        /// </summary>
        /// <seealso cref="Next"/>
        [NotNull] private readonly WeakReference<Layer> _nextLayer;

        /// <summary>
        /// Gets the input layer.
        /// </summary>
        /// <value>The input layer.</value>
        /// <seealso cref="Next"/>
        public Layer Previous
        {
            [CanBeNull] get { return _previousLayer; }
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
        /// Gets the number of output neurons in this layer.
        /// </summary>
        /// <value>The number of output neurons.</value>
        public int NeuronCount
        {
            [Pure] get { return _weightMatrix.RowCount; }
        }

        /// <summary>
        /// Gets the number of inputs to this layer.
        /// </summary>
        /// <value>The number of inputs.</value>
        public int InputCount
        {
            [Pure] get { return _weightMatrix.ColumnCount; }
        }

        /// <summary>
        /// Gets the weights.
        /// </summary>
        /// <value>The weights.</value>
        [NotNull]
        public Matrix<float> Weights
        {
            [Pure] get { return _weightMatrix; }
        }

        /// <summary>
        /// Gets the bias.
        /// </summary>
        /// <value>The bias.</value>
        [NotNull]
        public Vector<float> Bias
        {
            [Pure]
            get { return _biasVector; }
        }

        /// <summary>
        /// Creates an empty weight matrix from the given <paramref name="layer"/>
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns>An empty weight matrix matching this layer.</returns>
        [NotNull]
        internal static Matrix<float> EmptyWeightFromLayer([NotNull] Layer layer)
        {
            var rows = layer.NeuronCount;
            var cols = layer.InputCount;
            return Matrix<float>.Build.Dense(rows, cols, Matrix<float>.Zero);
        }

        /// <summary>
        /// Creates an empty bias vector from the given <paramref name="layer"/>
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns>An empty bias vector matching this layer.</returns>
        [NotNull]
        internal static Vector<float> EmptyBiasFromLayer([NotNull] Layer layer)
        {
            var rows = layer.NeuronCount;
            return Vector<float>.Build.Dense(rows, Matrix<float>.Zero);
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
            _derivativeFunction = transferFunction.Derivative;
        }

        /// <summary>
        /// Performs a feed-forward step of the layer's <paramref name="input"/>.
        /// </summary>
        /// <param name="input">The row vector of inputs.</param>
        /// <returns>The activations of this layer's perceptrons.</returns>
        [Pure]
        public FeedforwardResult Feedforward([NotNull] Vector<float> input)
        {
            // calculate activations for each neuron in the layer
            var activation = _weightMatrix*input + _biasVector;

            // apply the activation function to each weighted activation
            var output = _transferFunction(activation);

            return new FeedforwardResult(this, activation, output);
        }

        /// <summary>
        /// Performs a backpropagation step of the layer's <paramref name="outputErrors" />.
        /// </summary>
        /// <param name="feeforwardResult">The layer's feedforward result.</param>
        /// <param name="outputErrors">The training errors.</param>
        /// <returns>The activations of this layer's perceptrons.</returns>
        /// <exception cref="System.InvalidOperationException">Attempted to backpropagate through the input layer.</exception>
        [Pure, NotNull]
        public BackpropagationResult Backpropagate(FeedforwardResult feeforwardResult, [NotNull] Vector<float> outputErrors)
        {
            if (Type != LayerType.Hidden) throw new InvalidOperationException("Backpropagation only allowed on hidden layers.");

            // calculate the gradient of the transfer function.
            // This function will fail on the input layer.
            var weightedInputs = feeforwardResult.WeightedInputs;
            var activations = feeforwardResult.Output;
            var gradient = _derivativeFunction(weightedInputs, activations);

            // In case of the output layer, the error is trivially
            // the difference of expected and calculated outputs,
            // so nothing needs to be done.
            // On hidden layers, the error is the weighted sum
            // of the errors to each originating neuron.

            // sum errors weighted by connection weights
            var transposedMatrix = Next._weightMatrix.Transpose();
            var weightError = (transposedMatrix * outputErrors).PointwiseMultiply(gradient);

            // calculate the bias error for the current layer
            // in analogy to the weight error calculaction above; however, since
            // the bias unit is not affected by the nonlinear transfer function,
            // it has linear effect, hence the multiplication with b' = 1.
            var biasError = (Next._biasVector * outputErrors) * 1.0F;

            return new BackpropagationResult(weightError, biasError);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            var inputs = _weightMatrix.ColumnCount;
            var outputs = _weightMatrix.RowCount;
            return string.Format("{0}: {1} --> {2}", Type, inputs, outputs);
        }
    }
}
