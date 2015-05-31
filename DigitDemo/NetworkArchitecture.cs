using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Widemeadows.MachineLearning.Neural.Perceptron;

namespace Widemeadows.MachineLearning.Neural.Demonstration.Digit
{
    /// <summary>
    /// Class NetworkArchitecture.
    /// </summary>
    sealed class NetworkArchitecture
    {
        /// <summary>
        /// Froms the network.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <returns>NetworkArchitecture.</returns>
        [NotNull]
        public static NetworkArchitecture FromNetwork([NotNull] Network network)
        {
            var neuronCount = new List<int>(network.LayerCount);
            var hiddenLayers = new List<Layer>(network.LayerCount - 2);
            Layer outputLayer = null;

            foreach (var layer in network)
            {
                // store the number of output neurons
                neuronCount.Add(layer.NeuronCount);
                if (layer.Type == LayerType.Input) continue;

                var bias = layer.Bias.ToArray();
                var weights = layer.Weights.ToRowWiseArray();

                // register the layer
                var storedLayer = new Layer(layer.InputCount, layer.NeuronCount, bias, weights);
                if (layer.Type == LayerType.Hidden)
                {
                    hiddenLayers.Add(storedLayer);
                }
                else
                {
                    Debug.Assert(outputLayer == null, "outputLayer == null");
                    outputLayer = storedLayer;
                }
            }

            Debug.Assert(outputLayer != null, "outputLayer != null");
            return new NetworkArchitecture(neuronCount, hiddenLayers.Count == 0 ? null : hiddenLayers, outputLayer);
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("name", Order = -1, Required = Required.Default, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [CanBeNull]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the neuron count per layer.
        /// </summary>
        /// <value>The neurons.</value>
        [NotNull]
        [JsonProperty("neuronCounts", Order = 0, Required = Required.Always)]
        public IReadOnlyList<int> NeuronCount { get; set; }

        /// <summary>
        /// Gets or sets the hidden layers.
        /// </summary>
        /// <value>The hidden layers.</value>
        [JsonProperty("hiddenLayers", Order = 1, Required = Required.Default, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [CanBeNull]
        public IReadOnlyList<Layer> HiddenLayers { get; set; }

        /// <summary>
        /// Gets or sets the output layer.
        /// </summary>
        /// <value>The output layer.</value>
        [NotNull]
        [JsonProperty("outputLayer", Order = 2, Required = Required.Always)]
        public Layer OutputLayer { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkArchitecture"/> class.
        /// </summary>
        /// <param name="neuronCount">The neuron count.</param>
        /// <param name="hiddenLayers">The hidden layers.</param>
        /// <param name="outputLayer">The output layer.</param>
        public NetworkArchitecture([NotNull] IReadOnlyList<int> neuronCount, [CanBeNull] IReadOnlyList<Layer> hiddenLayers, [NotNull] Layer outputLayer)
        {
            NeuronCount = neuronCount;
            HiddenLayers = hiddenLayers;
            OutputLayer = outputLayer;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="NetworkArchitecture"/> class from being created.
        /// </summary>
        [UsedImplicitly, JsonConstructor]
        private NetworkArchitecture()
        {
        }

        /// <summary>
        /// A single layer.
        /// </summary>
        public class Layer
        {
            /// <summary>
            /// Gets or sets the inputs.
            /// </summary>
            /// <value>The inputs.</value>
            [JsonProperty("inputs", Order = 0, Required = Required.Always)]
            public int Inputs { get; set; }

            /// <summary>
            /// Gets or sets the outputs.
            /// </summary>
            /// <value>The outputs.</value>
            [JsonProperty("outputs", Order = 1, Required = Required.Always)]
            public int Outputs { get; set; }

            /// <summary>
            /// Gets or sets the bias.
            /// </summary>
            /// <value>The bias.</value>
            [JsonProperty("bias", Order = 2, Required = Required.Always)]
            public IReadOnlyList<float> Bias { get; set; }

            /// <summary>
            /// Gets or sets the weights.
            /// </summary>
            /// <value>The weights.</value>
            [JsonProperty("weights", Order = 3, Required = Required.Always)]
            public IReadOnlyList<float> Weights { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Layer"/> class.
            /// </summary>
            /// <param name="inputs">The inputs.</param>
            /// <param name="outputs">The outputs.</param>
            /// <param name="bias">The bias.</param>
            /// <param name="weights">The weights.</param>
            public Layer(int inputs, int outputs, IReadOnlyList<float> bias, IReadOnlyList<float> weights)
            {
                Inputs = inputs;
                Outputs = outputs;
                Bias = bias;
                Weights = weights;
            }

            /// <summary>
            /// Prevents a default instance of the <see cref="Layer"/> class from being created.
            /// </summary>
            [JsonConstructor, UsedImplicitly]
            private Layer()
            {

            }
        }
    }
}
