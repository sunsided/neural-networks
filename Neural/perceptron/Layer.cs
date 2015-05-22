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
        /// The weight matrix
        /// </summary>
        [NotNull]
        private readonly Matrix<float> _weightMatrix;

        /// <summary>
        /// The activation function
        /// </summary>
        [NotNull]
        private readonly Func<float, float> _activationFunction;

        /// <summary>
        /// The gradient function
        /// </summary>
        [NotNull]
        private readonly Func<float, float> _gradient;

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
            _activationFunction = activationFunction.Activate;
            _gradient = activationFunction.Derivative;
        }

        /// <summary>
        /// Struct FeedforwardResult
        /// </summary>
        public struct FeedforwardResult
        {
            /// <summary>
            /// The weighted sum of input activations.
            /// </summary>
            [NotNull] 
            public readonly Vector<float> Z;
            
            /// <summary>
            /// The activation value
            /// </summary>
            [NotNull] 
            public readonly Vector<float> Activation;

            /// <summary>
            /// Initializes a new instance of the <see cref="FeedforwardResult"/> struct.
            /// </summary>
            /// <param name="z">The z.</param>
            /// <param name="activation">The activation.</param>
            public FeedforwardResult([NotNull] Vector<float> z, [NotNull] Vector<float> activation)
            {
                Z = z;
                Activation = activation;
            }
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
            var outputActivations = weightedActivations.Map(activationFunction);

            return new FeedforwardResult(weightedActivations, outputActivations);
        }

        /// <summary>
        /// Performs a backpropagation step of the layer's <paramref name="errors" />.
        /// </summary>
        /// <param name="errors">The training errors.</param>
        /// <param name="weightedInputActivations">This layer's weighted input activations.</param>
        /// <returns>The activations of this layer's perceptrons.</returns>
        [Pure, NotNull] 
        public Vector<float> Backpropagate([NotNull] Vector<float> errors, Vector<float> weightedInputActivations)
        {           
            // calculate the gradient of the activation function
            var gradient = weightedInputActivations.Map(_gradient);

            // calculate the delta for the current layer
            var matrix = _weightMatrix.Transpose();
            errors = matrix * errors;
            return errors.PointwiseMultiply(gradient);
        }
    }
}
