using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
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
            var inputLayer = LayerConfiguration.ForInput(2);

            // one hidden layer with three neurons
            var hiddenLayers = new[]
                               {
                                   LayerConfiguration.ForHidden(3, activation)
                               };

            // output layer with one neuron
            var outputLayer = LayerConfiguration.ForOutput(1, activation);

            // construct a network
            var factory = new NetworkFactory();
            var network = factory.Create(inputLayer, hiddenLayers, outputLayer);

            // evaluate the network
            var inputs = new[]
                         {
                             0F, 1F
                         };
            
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Console.WriteLine("Evaluating network for input:");
            Console.WriteLine(String.Join(", ", inputs));

            var outputs = network.Calculate(inputs);

            Console.WriteLine("Obtained result from network:");
            Console.WriteLine(String.Join(", ", outputs));

            if (Debugger.IsAttached) Console.ReadKey(true);
        }
    }
}
