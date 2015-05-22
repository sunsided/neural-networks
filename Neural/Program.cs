using Neural.Activations;
using Neural.Perceptron;

namespace Neural
{
    /// <summary>
    /// Class Program.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            // The XOR problem

            // obtain an activation function
            var activation = new LinearActivation();

            // input layers with two neurons
            var inputLayer = new LayerConfiguration(2, activation);

            // one hidden layer with two neurons
            var hiddenLayers = new[]
                               {
                                   new LayerConfiguration(2, activation)
                               };

            // output layer with one neuron
            var outputLayer = new LayerConfiguration(1, activation);

            // construct a network
            var factory = new NetworkFactory();
            var network = factory.Create(inputLayer, hiddenLayers, outputLayer);
        }
    }
}
