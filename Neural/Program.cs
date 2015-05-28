using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
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

            // obtain a transfer function
            var activation = new SigmoidTransfer();

            // input layers with two neurons
            var inputLayer = LayerConfiguration.ForInput(400);

            // one hidden layer with three neurons
            var hiddenLayers = new[]
                               {
                                   LayerConfiguration.ForHidden(25, activation)
                               };

            // output layer with one neuron
            var outputLayer = LayerConfiguration.ForOutput(10, activation);

            // construct a network
            var factory = new NetworkFactory();
            var network = factory.Create(inputLayer, hiddenLayers, outputLayer);

            // train the network
            var examples = new[]
                           {
                               new TrainingExample(Vector<float>.Build.Random(400), Vector<float>.Build.Random(10)),
                               new TrainingExample(Vector<float>.Build.Random(400), Vector<float>.Build.Random(10)),
                               new TrainingExample(Vector<float>.Build.Random(400), Vector<float>.Build.Random(10)),
                               new TrainingExample(Vector<float>.Build.Random(400), Vector<float>.Build.Random(10))
                           };

            network.Train(examples);

            // evaluate the network
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Console.WriteLine("Evaluating network for input:");
            Console.WriteLine(String.Join(", ", examples[0].Inputs));

            var outputs = network.Calculate(examples[0].Inputs);

            Console.WriteLine("Obtained result from network:");
            Console.WriteLine(String.Join(", ", outputs));

            if (Debugger.IsAttached) Console.ReadKey(true);
        }
    }
}
