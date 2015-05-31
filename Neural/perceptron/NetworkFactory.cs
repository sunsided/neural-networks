using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using Neural.Activations;

namespace Neural.Perceptron
{
    /// <summary>
    /// Class PerceptronNetworkFactory. This class cannot be inherited.
    /// </summary>
    public sealed class NetworkFactory
    {
        /// <summary>
        /// Creates the specified input layer configuration.
        /// </summary>
        /// <param name="inputLayerConfiguration">The input layer configuration.</param>
        /// <param name="outputLayerConfiguration">The output layer configuration.</param>
        /// <returns>PerceptronNetwork.</returns>
        public Network Create(LayerConfiguration inputLayerConfiguration, LayerConfiguration outputLayerConfiguration)
        {
            var layerConfigurations = new[]
                                      {
                                          inputLayerConfiguration,
                                          outputLayerConfiguration
                                      };

            return Create(layerConfigurations);
        }

        /// <summary>
        /// Creates the specified input layer configuration.
        /// </summary>
        /// <param name="inputLayerConfiguration">The input layer configuration.</param>
        /// <param name="hiddenLayerConfigurations">The hidden layer configurations.</param>
        /// <param name="outputLayerConfiguration">The output layer configuration.</param>
        /// <returns>PerceptronNetwork.</returns>
        public Network Create(LayerConfiguration inputLayerConfiguration, [NotNull] IReadOnlyList<LayerConfiguration> hiddenLayerConfigurations, LayerConfiguration outputLayerConfiguration)
        {
            var layerConfigurations = new List<LayerConfiguration>(hiddenLayerConfigurations.Count + 2);
            layerConfigurations.Add(inputLayerConfiguration);
            layerConfigurations.AddRange(hiddenLayerConfigurations);
            layerConfigurations.Add(outputLayerConfiguration);

            return Create(layerConfigurations);
        }

        /// <summary>
        /// Creates the specified input layer configuration.
        /// </summary>
        /// <param name="layerConfigurations">The layer configurations.</param>
        /// <returns>PerceptronNetwork.</returns>
        private Network Create<T>([NotNull] T layerConfigurations)
            where T : IReadOnlyList<LayerConfiguration>
        {
            // Prepare a linked list of perceptron layers
            var layerList = new LinkedList<Layer>();

            var inputLayerConfiguration = layerConfigurations.First();
            var nonInputLayerConfigurations = layerConfigurations.Skip(1);

            // As the input layer has no previous layer, we initialize this as null.
            var previousNextLayer = new WeakReference<Layer>(null);

            // This value encodes the number of neurons in the previous layer
            // that act as an input to each perceptron within this layer.
            // For the input layer, each input is directly assigned;
            // we encode this as one (virtual) input neuron per perceptron.
            var inputNeurons = inputLayerConfiguration.NeuronCount;

            // the first layer is of purely cosmetic nature:
            // although the input layer does not do anything, it is still
            // considered a layer, so it is added here with linear activation,
            // zero bias and linear transfer.
            var biasVector = Vector<float>.Build.Dense(inputNeurons, Matrix<float>.Zero);
            var weightMatrix = Matrix<float>.Build.Diagonal(inputNeurons, inputNeurons, Matrix<float>.One);
            var inputLayerTransfer = new InputLayerTransfer();

            var inputLayer = new Layer(null, previousNextLayer, biasVector, weightMatrix, inputLayerTransfer);
            layerList.AddLast(inputLayer);

            // for the first iteration we initialize the previous layer as the input layer
            Layer previousLayer = inputLayer;

            // prepare a random distribution
            var dist = new Normal(0, 0.5);

            // We now iterate over all configurations and create weight vectors
            // for each perceptron according to the number of input neurons, where
            // each weight is initialized with a random value.
            // For efficient calculation, the weights of all nerons in a given layer
            // are stored as rows of a weight matrix.
            foreach (var layerConfiguration in nonInputLayerConfigurations)
            {
                var layerNeurons = layerConfiguration.NeuronCount;
                var activation = layerConfiguration.ActivationFunction;

                // set weights and biases
                weightMatrix = layerConfiguration.Weights;
                biasVector = layerConfiguration.Bias;
                if (weightMatrix == null || biasVector == null)
                {
                    biasVector = Vector<float>.Build.Random(layerNeurons, dist);
                    weightMatrix = Matrix<float>.Build.Random(layerNeurons, inputNeurons, dist);
                }

                // we create a new weak reference to the following layer
                // that will be wired up accordingly in the next iteration.
                var nextLayer = new WeakReference<Layer>(null);

                // creation and registration of the layer
                var layer = new Layer(previousLayer, nextLayer, biasVector, weightMatrix, activation);
                layerList.AddLast(layer);

                // update the "next layer" reference in the previous layer,
                // then rewire the reference to the new instance created above
                // so that the next iteration can update it accordingly.
                previousNextLayer.SetTarget(layer);
                previousNextLayer = nextLayer;

                // We now store the number of neurons in this layer
                // as the number of input neurons of the next layer
                // and rewire the reference of the previous layer.
                inputNeurons = layerNeurons;
                previousLayer = layer;
            }

            return new Network(layerList);
        }
    }
}
