using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using MathNet.Numerics.LinearAlgebra;
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
            var inputs = Vector<float>.Build.Dense(new[]
                                                   {
                                                       0F, 1F
                                                   });
            Console.WriteLine("Evaluating network for input:");
            Console.WriteLine(inputs.ToVectorString("G2", CultureInfo.InvariantCulture));

            var outputs = network.Calculate(inputs);
            Console.WriteLine("Obtained result from network:");
            Console.WriteLine(outputs.ToVectorString("G2", CultureInfo.InvariantCulture));

            if (Debugger.IsAttached) Console.ReadKey(true);
        }
    }
}
