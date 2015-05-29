using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;
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
            var inputLayer = LayerConfiguration.ForInput(3);

            // one hidden layer with three neurons
            var weights = Matrix<float>.Build.DenseOfColumnArrays(
                new[]
                {
                    -0.0279415498198926F,
                    0.0656986598718789F,
                    0.0989358246623382F,
                    0.0412118485241757F,
                    -0.054402111088937F
                },
                new[]
                {
                    -0.0999990206550704F,
                    -0.0536572918000435F,
                    0.0420167036826641F,
                    0.099060735569487F,
                    0.0650287840157117F
                },
                new[]
                {
                    -0.0287903316665065F,
                    -0.0961397491879557F,
                    -0.0750987246771676F,
                    0.0149877209662952F,
                    0.0912945250727628F
                }
                );

            var bias = Vector<float>.Build.DenseOfArray(
                new[]
                {
                    0.0841470984807896F,
                    0.0909297426825682F,
                    0.0141120008059867F,
                    -0.0756802495307928F,
                    -0.0958924274663139F
                });

            var hiddenLayers = new[]
                               {
                                   LayerConfiguration.ForHidden(activation, weights, bias)
                               };

            // output layer with one neuron
            weights = Matrix<float>.Build.DenseOfColumnArrays(
                new[]
                {
                    -0.0756802495307928F,
                    -0.0958924274663139F,
                    -0.0279415498198926F
                },
                new[]
                {
                    0.0656986598718789F,
                    0.0989358246623382F,
                    0.0412118485241757F
                },
                new[]
                {
                    -0.054402111088937F,
                    -0.0999990206550704F,
                    -0.0536572918000435F
                },
                new[]
                {
                    0.0420167036826641F,
                    0.099060735569487F,
                    0.0650287840157117F
                },
                new[]
                {
                    -0.0287903316665065F,
                    -0.0961397491879557F,
                    -0.0750987246771676F
                }
                );

            bias = Vector<float>.Build.DenseOfArray(
                new[]
                {
                    0.0841470984807896F,
                    0.0909297426825682F,
                    0.0141120008059867F
                });

            var outputLayer = LayerConfiguration.ForOutput(activation, weights, bias);

            // construct a network
            var factory = new NetworkFactory();
            var network = factory.Create(inputLayer, hiddenLayers, outputLayer);

            // train the network
            var examples = new[]
                           {
                               new TrainingExample(Vector<float>.Build.DenseOfArray(new[] {0.0841470984807896F, -0.0279415498198926F, -0.0999990206550704F}), Vector<float>.Build.DenseOfArray(new [] { 0F, 1F, 0F })),
                               new TrainingExample(Vector<float>.Build.DenseOfArray(new[] {0.0909297426825682F, 0.0656986598718789F, -0.0536572918000435F}), Vector<float>.Build.DenseOfArray((new [] { 0F, 0F, 1F }))),
                               new TrainingExample(Vector<float>.Build.DenseOfArray(new[] {0.0141120008059867F, 0.0989358246623382F, 0.0420167036826641F}), Vector<float>.Build.DenseOfArray((new [] { 1F, 0F, 0F }))),
                               new TrainingExample(Vector<float>.Build.DenseOfArray(new[] {-0.0756802495307928F, 0.0412118485241757F, 0.099060735569487F}), Vector<float>.Build.DenseOfArray((new [] { 0F, 1F, 0F }))),
                               new TrainingExample(Vector<float>.Build.DenseOfArray(new[] {-0.0958924274663139F, -0.054402111088937F, 0.0650287840157117F}), Vector<float>.Build.DenseOfArray((new [] { 0F, 0F, 1F })))
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
