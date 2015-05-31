using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using Neural.Activations;

namespace Neural.Perceptron
{
    /// <summary>
    /// Configuration of a single perceptron layer.
    /// </summary>
    public struct LayerConfiguration
    {
        /// <summary>
        /// Creates a configuration for the input layer.
        /// </summary>
        /// <param name="neuronCount">The neuron count.</param>
        /// <returns>LayerConfiguration.</returns>
        public static LayerConfiguration ForInput(int neuronCount)
        {
            return new LayerConfiguration(neuronCount, new InputLayerTransfer());
        }

        /// <summary>
        /// Creates a configuration for the hidden layer.
        /// </summary>
        /// <param name="neuronCount">The neuron count.</param>
        /// <param name="activation">The activation function.</param>
        /// <param name="weights">The weights.</param>
        /// <returns>LayerConfiguration.</returns>
        public static LayerConfiguration ForHidden(int neuronCount, [NotNull] ITransfer activation)
        {
            return new LayerConfiguration(neuronCount, activation);
        }

        /// <summary>
        /// Creates a configuration for the hidden layer.
        /// </summary>
        /// <param name="neuronCount">The neuron count.</param>
        /// <param name="activation">The activation function.</param>
        /// <param name="weights">The weights.</param>
        /// <returns>LayerConfiguration.</returns>
        public static LayerConfiguration ForHidden([NotNull] ITransfer activation, [NotNull] Matrix<float> weights, [NotNull] Vector<float> bias)
        {
            return new LayerConfiguration(weights.RowCount, activation, weights, bias);
        }

        /// <summary>
        /// Creates a configuration for the output layer.
        /// </summary>
        /// <param name="neuronCount">The neuron count.</param>
        /// <param name="activation">The activation function.</param>
        /// <param name="weights">The weights.</param>
        /// <returns>LayerConfiguration.</returns>
        public static LayerConfiguration ForOutput(int neuronCount, [NotNull] ITransfer activation)
        {
            return new LayerConfiguration(neuronCount, activation);
        }

        /// <summary>
        /// Creates a configuration for the output layer.
        /// </summary>
        /// <param name="neuronCount">The neuron count.</param>
        /// <param name="activation">The activation function.</param>
        /// <param name="weights">The weights.</param>
        /// <param name="bias">The bias.</param>
        /// <returns>LayerConfiguration.</returns>
        public static LayerConfiguration ForOutput([NotNull] ITransfer activation, [NotNull] Matrix<float> weights, [NotNull] Vector<float> bias)
        {
            return new LayerConfiguration(weights.RowCount, activation, weights, bias);
        }

        /// <summary>
        /// Gets the number of neurons in this layer.
        /// </summary>
        /// <value>The neuron count.</value>
        public readonly int NeuronCount;

        /// <summary>
        /// Gets the activation function of all neurons in the layer.
        /// </summary>
        [NotNull]
        public readonly ITransfer ActivationFunction;

        /// <summary>
        /// The weights
        /// </summary>
        [CanBeNull]
        public readonly Matrix<float> Weights;

        /// <summary>
        /// The biases
        /// </summary>
        [CanBeNull]
        public readonly Vector<float> Bias;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerConfiguration" /> class.
        /// </summary>
        /// <param name="neuronCount">The number of neurons in this layer.</param>
        /// <param name="activationFunction">The activation function.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of neurons was negative or zero.</exception>
        public LayerConfiguration(int neuronCount, [NotNull] ITransfer activationFunction, [CanBeNull] Matrix<float> weights, [CanBeNull] Vector<float> bias)
        {
            if (neuronCount <= 0) throw new ArgumentOutOfRangeException("neuronCount", neuronCount, "The number of neurons must be positive.");
            if (weights != null && weights.RowCount != neuronCount) throw new ArgumentException("The weight matrix row count must match the layer's neuron count");
            if (bias != null && bias.Count != neuronCount) throw new ArgumentException("The bias vector size must match the layer's neuron count");

            NeuronCount = neuronCount;
            ActivationFunction = activationFunction;
            Weights = weights;
            Bias = bias;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerConfiguration" /> class.
        /// </summary>
        /// <param name="neuronCount">The number of neurons in this layer.</param>
        /// <param name="activationFunction">The activation function.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of neurons was negative or zero.</exception>
        public LayerConfiguration(int neuronCount, [NotNull] ITransfer activationFunction)
            : this(neuronCount, activationFunction, null, null)
        {
        }
    }
}
