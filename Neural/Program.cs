﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using MathNet.Numerics.LinearAlgebra;
using Neural.Activations;
using Neural.Perceptron;
using Neural.Training;

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
            // The XOR problem, adapted from Clever Algorithms by Jason Brownlee

            // obtain a transfer function
            var hiddenActivation = new TanhTransfer();
            var outputActivation = new StepTransfer
                                   {
                                       Epsilon = 1E-7F
                                   };

            // input layers with two neurons
            var inputLayer = LayerConfiguration.ForInput(2);

            // single hidden layer with two neurons
            var hiddenLayers = new[]
                               {
                                   LayerConfiguration.ForHidden(2, hiddenActivation)
                               };

            // output layer with one neuron
            var outputLayer = LayerConfiguration.ForOutput(1, outputActivation);

            // construct a network
            var factory = new NetworkFactory();
            var network = factory.Create(inputLayer, hiddenLayers, outputLayer);

            // train the network
            var examples = new[]
                           {
                               new TrainingExample(Vector<float>.Build.DenseOfArray(new[] {0F, 0F}), Vector<float>.Build.DenseOfArray(new [] { 0F })),
                               new TrainingExample(Vector<float>.Build.DenseOfArray(new[] {0F, 1F}), Vector<float>.Build.DenseOfArray((new [] { 1F }))),
                               new TrainingExample(Vector<float>.Build.DenseOfArray(new[] {1F, 0F}), Vector<float>.Build.DenseOfArray((new [] { 1F }))),
                               new TrainingExample(Vector<float>.Build.DenseOfArray(new[] {1F, 1F}), Vector<float>.Build.DenseOfArray((new [] { 0F }))),
                           };

            // select a training strategy
            var training = new MomentumDescend()
                           {
                               LearningRate = 0.3F,
                               Momentum = 0.8F,
                               MaximumIterationCount = 2000,
                               RegularizationStrength = 0
                           };

            network.Train(training, examples);

            // evaluate the network
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            foreach (var example in examples)
            {
                Console.WriteLine("Evaluating network for input:");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(String.Join(", ", example.Inputs));
                Console.ResetColor();

                Console.WriteLine("Expected result from network:");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(String.Join(", ", example.Outputs));
                Console.ResetColor();

                var outputs = network.Calculate(example.Inputs);

                Console.WriteLine("Obtained result from network:");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(String.Join(", ", outputs));
                Console.ResetColor();

                Console.WriteLine();
            }

            if (Debugger.IsAttached) Console.ReadKey(true);
        }
    }
}
