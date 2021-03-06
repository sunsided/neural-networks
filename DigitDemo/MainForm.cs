﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using Newtonsoft.Json;
using Widemeadows.MachineLearning.Neural.Activations;
using Widemeadows.MachineLearning.Neural.Cost;
using Widemeadows.MachineLearning.Neural.Perceptron;
using Widemeadows.MachineLearning.Neural.Training;
using Widemeadows.MachineLearning.Neural.Training.Backpropagation;

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
        private MomentumDescent _networkTraining;

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

            ResetNetwork();
            _networkTraining = GenerateNetworkTraining();
        }

        #region Network Generation

        /// <summary>
        /// Generates the network trainer.
        /// </summary>
        /// <returns>ITraining.</returns>
        [NotNull]
        private MomentumDescent GenerateNetworkTraining()
        {
            // select a backpropagation algorithm
            var bp = new DefaultBackpropagation
                     {
                         FlatSpotElimination = 0.1F
                     };

            // select a cost function
            var cost = new LogisticCost(bp);

            // select a training strategy
            return new MomentumDescent(cost)
            {
                LearningRate = 0.5F,
                Momentum = 0.8F,
                MaximumIterationCount = 600,
                MinimumIterationCount = 10,
                RegularizationStrength = 0.03F
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

        /// <summary>
        /// Generates the network.
        /// </summary>
        [NotNull]
        private Network GenerateNetwork(NetworkArchitecture architecture)
        {
            if (architecture.NeuronCount[0] != 400) throw new ArgumentException("Network input neuron count must be 400");
            if (architecture.NeuronCount[1] != 25) throw new ArgumentException("First hidden layer input neuron count must be 25");
            if (architecture.NeuronCount.Last() != 10) throw new ArgumentException("Network output neuron count must be 10");

            // obtain a transfer function
            ITransfer hiddenActivation = new SigmoidTransfer();
            ITransfer outputActivation = new SigmoidTransfer();

            // input layers with 400 (20x20) neurons
            var inputLayer = LayerConfiguration.ForInput(architecture.NeuronCount[0]);

            // one hidden layer with 25 (5x5) neurons
            var hiddenLayers = new List<LayerConfiguration>();
            if (architecture.HiddenLayers != null)
            {
                for (int i = 0; i < architecture.HiddenLayers.Count; ++i)
                {
                    var layer = architecture.HiddenLayers[i];
                    var cols = layer.Inputs;
                    var rows = layer.Outputs;

                    var weights = Matrix<float>.Build.Dense(rows, cols, layer.Weights.ToArray());
                    var bias = Vector<float>.Build.DenseOfArray(layer.Bias.ToArray());
                    var conf = LayerConfiguration.ForHidden(hiddenActivation, weights, bias);
                    hiddenLayers.Add(conf);
                }
            }

            // output layer with one neuron
            var outLayer = architecture.OutputLayer;
            var outCols = outLayer.Inputs;
            var outRows = outLayer.Outputs;

            var outWeights = Matrix<float>.Build.Dense(outRows, outCols, outLayer.Weights.ToArray());
            var outBias = Vector<float>.Build.DenseOfArray(outLayer.Bias.ToArray());
            var outputLayer = LayerConfiguration.ForHidden(outputActivation, outWeights, outBias);

            // construct a network
            var factory = new NetworkFactory();
            return factory.Create(inputLayer, hiddenLayers, outputLayer);
        }

        #endregion Network Generation

        /// <summary>
        /// Resets the network.
        /// </summary>
        private void ResetNetwork()
        {
            _network = GenerateNetwork();
            ActivateNetwork();
        }

        private void ActivateNetwork()
        {
            var example = _currentExample;
            if (example != null)
            {
                EvaluateAndPresentExample(example.Value);
            }

            EvaluateNetworkPerformance();
        }

        /// <summary>
        /// Evaluates the network performance.
        /// </summary>
        /// <returns>Task.</returns>
        private async void EvaluateNetworkPerformance()
        {
            labelCrossValidationPerformance.Text = "-";
            labelTrainingDataPerformance.Text = "-";

            if (_testSet == null) return;
            var performance = await EvaluateNetworkPerformanceOnTestSet();
            labelCrossValidationPerformance.Text = performance.ToString("P");

            if (_trainingSet == null) return;
            performance = await EvaluateNetworkPerformanceOnTrainingSet();
            labelTrainingDataPerformance.Text = performance.ToString("P");
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Show();
            Update();
            var dialog = new ProgressDialog();
            dialog.Text = "Loading data ...";

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
            EvaluateNetworkPerformance();
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
            PresentOutput(example, result);
        }

        /// <summary>
        /// Presents the network's output.
        /// </summary>
        /// <param name="result">The result.</param>
        private int PresentOutput(LinkedList<FeedforwardResult> result)
        {
            var layerOutput = result.Last.Value;
            var outputs = layerOutput.Output;

            var label = outputs.MaximumIndex();
            var likelihood = outputs[label];

            labelNetworkOutput.Text = label.ToString();
            labelLikelihood.Text = likelihood.ToString("P");

            return label;
        }

        /// <summary>
        /// Presents the network's output.
        /// </summary>
        /// <param name="example">The example.</param>
        /// <param name="result">The result.</param>
        private void PresentOutput(TrainingExample example, LinkedList<FeedforwardResult> result)
        {
            var label = PresentOutput(result);
            var certainty = result.Last.Value.Output[label];

            var isMatch = label == example.Label;
            if (isMatch)
            {
                labelNetworkOutput.ForeColor = Color.DarkGreen;
                labelLikelihood.ForeColor = certainty > 0.8
                    ? Color.DarkGreen
                    : (certainty > 0.5) ? Color.DarkOrange : Color.DarkRed;
            }
            else
            {
                labelNetworkOutput.ForeColor = Color.DarkRed;
                labelLikelihood.ForeColor = certainty > 0.8
                    ? Color.DarkRed
                    : (certainty > 0.5) ? Color.DarkOrange : Color.DarkGreen;
            }
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
            var network = _network;
            var trainer = _networkTraining;

            var dialog = new TrainingProgressDialog(trainer.MaximumIterationCount);
            dialog.Text = "Training network ...";

            var cts = new CancellationTokenSource();
            dialog.TrainingCanceled += (s, a) => cts.Cancel();

            dialog.Shown += async (s, args) =>
                                  {
                                      // shuffle the set and create training examples
                                      var set = _trainingSet.Shuffle();
                                      var examples = set.AsParallel().Select(MakeTrainingExample).ToList();

                                      // prepare progress reporting
                                      var progress = new Progress<TrainingProgress>();
                                      progress.ProgressChanged += (_, p) => dialog.UpdateProgress(p);

                                      // trainify!
                                      var stop = await Task.Run(() => network.Train(trainer, examples, progress, cts.Token), cts.Token);
                                      dialog.Close();

                                      // evaluatify!
                                      EvaluateNetworkPerformance();

                                      var example = _currentExample;
                                      if (example != null) EvaluateAndPresentExample(example.Value);
                                  };
            dialog.ShowDialog(this);
        }

        /// <summary>
        /// Makes the training example.
        /// </summary>
        /// <param name="trainingExample">The training example.</param>
        /// <returns>Widemeadows.MachineLearning.Neural.Perceptron.TrainingExample.</returns>
        [Pure]
        private Perceptron.TrainingExample MakeTrainingExample(TrainingExample trainingExample)
        {
            var inputs = Vector<float>.Build.DenseOfEnumerable(trainingExample.Pixels.Enumerate());
            var outputs = Vector<float>.Build.Dense(10, Vector<float>.Zero);
            outputs[trainingExample.Label] = 1.0F;

            return new Perceptron.TrainingExample(inputs, outputs);
        }

        /// <summary>
        /// Handles the Click event of the toolStripMenuItemResetNetwork control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void toolStripMenuItemResetNetwork_Click(object sender, EventArgs e)
        {
            ResetNetwork();
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

        /// <summary>
        /// Handles the Click event of the toolStripButtonRandomExample control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void toolStripButtonRandomExample_Click(object sender, EventArgs e)
        {
            EvaluateAndPresentRandomExampleFromTestSet();
        }

        /// <summary>
        /// Evaluates the network performance on the test set.
        /// </summary>
        /// <returns>System.Single.</returns>
        private Task<float> EvaluateNetworkPerformanceOnTestSet()
        {
            var set = _testSet;
            return Task.Run(() => EvaluateNetworkPerformance(set));
        }

        /// <summary>
        /// Evaluates the network performance on the test set.
        /// </summary>
        /// <returns>System.Single.</returns>
        private Task<float> EvaluateNetworkPerformanceOnTrainingSet()
        {
            var set = _trainingSet;
            return Task.Run(() => EvaluateNetworkPerformance(set));
        }

        /// <summary>
        /// Evaluates the network performance on the given set.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>System.Single.</returns>
        private float EvaluateNetworkPerformance(IReadOnlyCollection<TrainingExample> set)
        {
            var correctMatches = (from example in set.AsParallel()
                let result = EvaluateNetwork(example)
                let output = result.Last.Value.Output
                let label = output.MaximumIndex()
                select label == example.Label ? 1 : 0)
                .Sum();

            return correctMatches/(float)set.Count;
        }

        /// <summary>
        /// Handles the Click event of the loadNetworkToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void loadNetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                AddExtension = true,
                AutoUpgradeEnabled = true,
                CheckFileExists = false,
                CheckPathExists = true,
                DefaultExt = ".mlp.json",
                DereferenceLinks = true,
                Filter = "Network files (*.mlp.json)|*.mlp.json|Alle Dateien|*.*",
                RestoreDirectory = true,
                SupportMultiDottedExtensions = true,
                Title = "Save network"
            };

            do
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                string json;
                using (var stream = File.Open(dialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    json = await reader.ReadToEndAsync();
                }

                var architecture = JsonConvert.DeserializeObject<NetworkArchitecture>(json);

                // check the network and load again
                if (architecture.NeuronCount[0] != 400 ||
                    architecture.NeuronCount[1] != 25 ||
                    architecture.NeuronCount.Last() != 10)
                {
                    var result = MessageBox.Show("The loaded network architecture is incompatible with this demonstration software.", "Incompatible Network", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.Cancel) return;
                    continue;
                }

                // construct the network
                _network = GenerateNetwork(architecture);
                ActivateNetwork();
                break;

            } while (true);
        }

        /// <summary>
        /// Handles the Click event of the saveNetworkToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void saveNetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
                         {
                             AddExtension = true,
                             AutoUpgradeEnabled = true,
                             CheckFileExists = false,
                             CheckPathExists = true,
                             CreatePrompt = false,
                             DefaultExt = ".mlp.json",
                             DereferenceLinks = true,
                             Filter = "Network files (*.mlp.json)|*.mlp.json|Alle Dateien|*.*",
                             OverwritePrompt = true,
                             RestoreDirectory = true,
                             SupportMultiDottedExtensions = true,
                             Title = "Save network"
                         };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            var architecture = await Task.Run(() => NetworkArchitecture.FromNetwork(_network));
            architecture.Name = "Handwritten Digits Classification";

            var json = JsonConvert.SerializeObject(architecture, Formatting.Indented);

            using (var stream = File.Open(dialog.FileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                await writer.WriteAsync(json);
            }
        }
    }
}
