using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using Widemeadows.MachineLearning.Neural.Perceptron;

namespace Widemeadows.MachineLearning.Neural.Training.Backpropagation
{
    /// <summary>
    /// Class DefaultBackpropagationLearning.
    /// </summary>
    internal abstract class DefaultBackpropagationLearning
    {
        /// <summary>
        /// The default flat spot elimination amount
        /// </summary>
        private const float DefaultFlatSpotElimination = 0.1F;

        /// <summary>
        /// The flat spot elimination amount
        /// </summary>
        private float _flatSpotElimination = DefaultFlatSpotElimination;

        /// <summary>
        /// Gets or sets the flat spot elimination amount.
        /// </summary>
        /// <value>The regularization strength.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Flat spot elimination parameter must be nonnegative</exception>
        /// <exception cref="System.NotFiniteNumberException">Flat spot elimination parameter must be a finite number</exception>
        public float FlatSpotElimination
        {
            get { return _flatSpotElimination; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value", value, "Regularization parameter must be nonnegative");
                if (double.IsInfinity(value) || double.IsNaN(value)) throw new NotFiniteNumberException("Regularization parameter must be a finite number", value);
                _flatSpotElimination = value;
            }
        }

        /// <summary>
        /// Performs a backpropagation step of the layer's <paramref name="outputErrors" />.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="feeforwardResult">The layer's feedforward result.</param>
        /// <param name="outputErrors">The training errors.</param>
        /// <returns>The activations of this layer's perceptrons.</returns>
        /// <exception cref="System.InvalidOperationException">Attempted to backpropagate through the input layer.</exception>
        [Pure]
        protected BackpropagationResult Backpropagate([NotNull] Layer layer, FeedforwardResult feeforwardResult, [NotNull] Vector<float> outputErrors)
        {
            if (layer.Type != LayerType.Hidden) throw new InvalidOperationException("Backpropagation only allowed on hidden layers.");

            // calculate the gradient of the transfer function.
            // This function will fail on the input layer.
            var weightedInputs = feeforwardResult.WeightedInputs;
            var activations = feeforwardResult.Output;
            var fse = _flatSpotElimination;

            var transferFunction = layer.TransferFunction;
            var gradient = transferFunction.Derivative(weightedInputs, activations) + fse;

            // In case of the output layer, the error is trivially
            // the difference of expected and calculated outputs,
            // so nothing needs to be done.
            // On hidden layers, the error is the weighted sum
            // of the errors to each originating neuron.

            var next = layer.Next;

            // sum errors weighted by connection weights
            var transposedMatrix = next.Weights.Transpose();
            var weightError = (transposedMatrix * outputErrors).PointwiseMultiply(gradient);

            // calculate the bias error for the current layer
            // in analogy to the weight error calculaction above; however, since
            // the bias unit is not affected by the nonlinear transfer function,
            // it has linear effect, hence the multiplication with b' = 1.
            var biasError = (next.Bias * outputErrors) * 1.0F;

            return new BackpropagationResult(weightError, biasError);
        }
    }
}
