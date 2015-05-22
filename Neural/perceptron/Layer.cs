using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using Neural.Activations;

namespace Neural.Perceptron
{
    /// <summary>
    /// A perceptron layer.
    /// </summary>
    sealed class Layer
    {
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
        /// <param name="weightMatrix">The weight matrix.</param>
        /// <param name="activationFunction">The activation function.</param>
        public Layer([NotNull] Matrix<float> weightMatrix, [NotNull] IActivation activationFunction)
        {
            _weightMatrix = weightMatrix;
            _activationFunction = activationFunction.Activate;
        }

        /// <summary>
        /// Performs a feed-forward step of the layer's <paramref name="activations"/>.
        /// </summary>
        /// <param name="activations">The row vector of activations.</param>
        /// <returns>The activations of this layer's perceptrons.</returns>
        [Pure, NotNull] 
        public Vector<float> Feedforward([NotNull] Vector<float> activations)
        {
            var matrix = _weightMatrix;
            var activationFunction = _activationFunction;

            // calculate the sum of weighted activations for each neuron in the layer
            var weighedActivations = matrix * activations;

            // apply the activation function to each weighted activation
            return weighedActivations.Map(activationFunction);
        }
    }
}
