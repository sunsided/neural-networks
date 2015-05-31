﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using Widemeadows.MachineLearning.Neural.Activations;
using Widemeadows.MachineLearning.Neural.Cost;
using Widemeadows.MachineLearning.Neural.Perceptron;
using Widemeadows.MachineLearning.Neural.Training;

namespace Widemeadows.MachineLearning.Neural.Demonstration.Digit
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// The MLP network
        /// </summary>
        [NotNull]
        private Network _network;

        /// <summary>
        /// The network training
        /// </summary>
        [NotNull]
        private ITraining _networkTraining;

        /// <summary>
        /// The randomizer
        /// </summary>
        readonly Random _rand = new Random();

        /// <summary>
        /// The training set
        /// </summary>
        [CanBeNull]
        private IReadOnlyCollection<TrainingExample> _trainingSet;

        /// <summary>
        /// The test set
        /// </summary>
        [CanBeNull]
        private IReadOnlyCollection<TrainingExample> _testSet;

        /// <summary>
        /// The currently selected example
        /// </summary>
        [CanBeNull]
        private TrainingExample? _currentExample;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            _network = GenerateNetwork();
            _networkTraining = GenerateNetworkTraining();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Show();
            var dialog = new ProgressDialog();

            IReadOnlyCollection<TrainingExample> data = null;
            dialog.Shown += async (sender, args) =>
                            {
                                var progress = new Progress<float>();
                                progress.ProgressChanged += (task, value) => dialog.UpdateProgress(value);

                                data = await Program.LoadTrainingExamplesAsync(progress);
                                dialog.Close();
                            };
            dialog.ShowDialog(this);
            Debug.Assert(data != null, "data != null");

            RegisterTrainingData(data);
        }

        /// <summary>
        /// Presents the training data.
        /// </summary>
        /// <param name="data">The data.</param>
        private void RegisterTrainingData(IReadOnlyCollection<TrainingExample> input)
        {
            var count = input.Count;
            var trainingSetSize = count*80/100;
            var testSetSize = count - trainingSetSize;

            var shuffled = input.Shuffle();
            _trainingSet = shuffled.Take(trainingSetSize).ToList();
            _testSet = shuffled.Skip(trainingSetSize).ToList();

            EvaluateAndPresentRandomExampleFromTestSet();
        }

        /// <summary>
        /// Evaluates and present a random example from the test set.
        /// </summary>
        private void EvaluateAndPresentRandomExampleFromTestSet()
        {
            var example = SelectRandomFromTestSet();
            _currentExample = example;

            EvaluateAndPresentExample(example);
        }

        /// <summary>
        /// Selects a random training example from the test set.
        /// </summary>
        /// <returns>TrainingExample.</returns>
        private TrainingExample SelectRandomFromTestSet()
        {
            var set = _testSet;
            if (set == null) throw new InvalidOperationException("Test set was null");

            var toSkip = _rand.Next(0, set.Count);
            var example = set.Skip(toSkip).Take(1).Single();
            return example;
        }

        /// <summary>
        /// Evaluates and presents a single example.
        /// </summary>
        /// <param name="example">The example.</param>
        private void EvaluateAndPresentExample(TrainingExample example)
        {
            var result = EvaluateNetwork(example);

            PresentTrainingInput(example);
            PresentFirstHiddenInput(result);
            PresentOutput(result);
        }

        /// <summary>
        /// Presents the network's output.
        /// </summary>
        /// <param name="result">The result.</param>
        private void PresentOutput(LinkedList<FeedforwardResult> result)
        {
            var layerOutput = result.Last.Value;
            var outputs = layerOutput.Output;

            var label = outputs.MaximumIndex();
            var likelihood = outputs[label];

            labelNetworkOutput.Text = label.ToString();
            labelLikelihood.Text = likelihood.ToString("P");
        }

        /// <summary>
        /// Presents the inputs of the first hidden layer.
        /// </summary>
        /// <param name="result">The result.</param>
        private void PresentFirstHiddenInput([NotNull] LinkedList<FeedforwardResult> result)
        {
            var layerResult = result.First.Next.Value;
            var inputs = layerResult.WeightedInputs;

            var bitmap = CreateBitmap(inputs, 5, 5);
            bitmap = NearestNeighborExtrapolate(bitmap, 32, 32);

            pictureBoxHidden.Image = bitmap;
        }

        /// <summary>
        /// Presents the entry.
        /// </summary>
        /// <param name="example">The example.</param>
        private void PresentTrainingInput(TrainingExample example)
        {
            var bitmap = CreateBitmap(example);
            bitmap = NearestNeighborExtrapolate(bitmap, pictureBoxDigit.Width, pictureBoxDigit.Height);

            labelClass.Text = example.Label.ToString();
            pictureBoxDigit.Image = bitmap;
        }

        /// <summary>
        /// Performs a nearest-neighbor extrapolation
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>Bitmap.</returns>
        private Bitmap NearestNeighborExtrapolate(Bitmap input, int width, int height)
        {
            var scaled = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            using (var gr = Graphics.FromImage(scaled))
            {
                gr.CompositingQuality = CompositingQuality.HighSpeed;
                gr.InterpolationMode = InterpolationMode.NearestNeighbor;
                gr.SmoothingMode = SmoothingMode.None;

                var destRect = new Rectangle(0, 0, width, height);
                var srcRect = new Rectangle(0, 0, input.Width, input.Height);
                gr.DrawImage(input, destRect, srcRect, GraphicsUnit.Pixel);
            }

            return scaled;
        }

        /// <summary>
        /// Creates the bitmap.
        /// </summary>
        /// <param name="example">The example.</param>
        /// <returns>Bitmap.</returns>
        [NotNull]
        private Bitmap CreateBitmap(TrainingExample example)
        {
            var bitmap = new Bitmap(20, 20, PixelFormat.Format32bppArgb);

            var min = example.Min;
            var max = example.Max;
            var pixels = example.Pixels.Map(v => (float) (255*(v - min)/(max - min)));

            foreach (var pixel in pixels.EnumerateIndexed())
            {
                var y = pixel.Item1;
                var x = pixel.Item2;
                var value = (int)Math.Floor(pixel.Item3);

                bitmap.SetPixel(x, y, Color.FromArgb(value, value, value));
            }

            return bitmap;
        }

        /// <summary>
        /// Creates the bitmap.
        /// </summary>
        /// <param name="inputActivations">The example.</param>
        /// <returns>Bitmap.</returns>
        [NotNull]
        private Bitmap CreateBitmap(Vector<float> inputActivations, int width, int height)
        {
            Debug.Assert(width * height == inputActivations.Count, "width * height == example.Count");
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            var min = inputActivations.Minimum();
            var max = inputActivations.Maximum();
            var pixels = inputActivations.Map(v => (float)(255 * (v - min) / (max - min)));

            foreach (var pixel in pixels.EnumerateIndexed())
            {
                var x = pixel.Item1 % width;
                var y = pixel.Item1 / width;
                var valueFloat = pixel.Item2;
                var value = (int)Math.Floor(valueFloat);

                bitmap.SetPixel(x, y, Color.FromArgb(value, value, value));
            }

            return bitmap;
        }

        /// <summary>
        /// Handles the ButtonClick event of the toolStripSplitButtonNetwork control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void toolStripSplitButtonNetwork_ButtonClick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Click event of the toolStripMenuItemResetNetwork control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void toolStripMenuItemResetNetwork_Click(object sender, EventArgs e)
        {
            _network = GenerateNetwork();

            var example = _currentExample;
            if (example == null) throw new InvalidOperationException("One example should be set");
            EvaluateAndPresentExample(example.Value);
        }

        #region Network evaluation

        /// <summary>
        /// Runs a single training example through the network.
        /// </summary>
        /// <param name="example">The example.</param>
        /// <returns>LinkedList&lt;FeedforwardResult&gt;.</returns>
        [NotNull]
        private LinkedList<FeedforwardResult> EvaluateNetwork(TrainingExample example)
        {
            var net = _network;

            var pixels = example.Pixels;
            var vector = Vector<float>.Build.DenseOfEnumerable(pixels.Enumerate(Zeros.Include));

            return net.Feedforward(vector);
        }

        #endregion Network evaluation

        #region Network Generation

        /// <summary>
        /// Generates the network trainer.
        /// </summary>
        /// <returns>ITraining.</returns>
        [NotNull]
        private ITraining GenerateNetworkTraining()
        {
            // select a cost function
            var cost = new SumSquaredErrorCost();

            // select a training strategy
            return new MomentumDescend(cost)
            {
                LearningRate = 0.5F,
                Momentum = 0.8F,
                MaximumIterationCount = 2000,
                MinimumIterationCount = 1000,
                RegularizationStrength = 0
            };
        }

        /// <summary>
        /// Generates the network.
        /// </summary>
        [NotNull]
        private Network GenerateNetwork()
        {
            // obtain a transfer function
            ITransfer hiddenActivation = new SigmoidTransfer();
            ITransfer outputActivation = new SigmoidTransfer();

            // input layers with 400 (20x20) neurons
            var inputLayer = LayerConfiguration.ForInput(400);

            // one hidden layer with 25 (5x5) neurons
            var hiddenLayers = new[]
                               {
                                   LayerConfiguration.ForHidden(25, hiddenActivation)
                               };

            // output layer with one neuron
            var outputLayer = LayerConfiguration.ForOutput(10, outputActivation);

            // construct a network
            var factory = new NetworkFactory();
            return factory.Create(inputLayer, hiddenLayers, outputLayer);
        }

        #endregion Network Generation

        /// <summary>
        /// Handles the Click event of the toolStripButtonRandomExample control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void toolStripButtonRandomExample_Click(object sender, EventArgs e)
        {
            EvaluateAndPresentRandomExampleFromTestSet();
        }
    }
}
