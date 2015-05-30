using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace Neural.Perceptron
{
    /// <summary>
    /// A perceptron network.
    /// </summary>
    sealed class PerceptronNetwork
    {
        /// <summary>
        /// The perceptron layers
        /// </summary>
        [NotNull]
        private readonly LinkedList<Layer> _layers;

        /// <summary>
        /// Gets the number of input neurons.
        /// </summary>
        /// <value>The number of input neurons.</value>
        public int InputNeuronCount
        {
            [Pure]
            get
            {
                var layer = _layers.First.Value;
                return layer.NeuronCount;
            }
        }

        /// <summary>
        /// Gets the number of output neurons.
        /// </summary>
        /// <value>The number of input neurons.</value>
        public int OutputNeuronCount
        {
            [Pure]
            get
            {
                var layer = _layers.Last.Value;
                return layer.NeuronCount;
            }
        }

        /// <summary>
        /// Gets the input layer.
        /// </summary>
        /// <value>The input layer.</value>
        internal Layer InputLayer
        {
            [Pure, NotNull]
            get { return _layers.First.Value; }
        }

        /// <summary>
        /// Gets the output layer.
        /// </summary>
        /// <value>The output layer.</value>
        internal Layer OutputLayer
        {
            [Pure, NotNull]
            get { return _layers.Last.Value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerceptronNetwork"/> class.
        /// </summary>
        /// <param name="layers">The perceptron layers.</param>
        public PerceptronNetwork([NotNull] LinkedList<Layer> layers)
        {
            _layers = layers;
        }

        /// <summary>
        /// Calculates the outputs given the specified <paramref name="inputs"/>.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        [Pure, NotNull]
        public IReadOnlyList<float> Calculate([NotNull] IReadOnlyList<float> inputs)
        {
            var inputVector = Vector<float>.Build.SparseOfEnumerable(inputs);
            var outputVector = CalculateInternalFast(inputVector);
            return outputVector.ToArray();
        }

        /// <summary>
        /// Calculates the network outputs given the specified <paramref name="input"/>.
        /// </summary>
        /// <param name="input">The inputs.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        [Pure, NotNull]
        private Vector<float> CalculateInternalFast([NotNull] Vector<float> input)
        {
            // Starting with the given inputs as the first hidden layer's activation,
            // iterate through all layers and calculate the next layer's activations.
            // The resulting activation of the last layer are the outputs.
            return _layers.Aggregate(input, (layerInput, layer) => layer.Feedforward(layerInput).Output);
        }

        /// <summary>
        /// Performs a feed-forward pass through the network and stores the results of each layer.
        /// </summary>
        /// <param name="input">The inputs.</param>
        /// <returns>LinkedList&lt;Layer.FeedforwardResult&gt;.</returns>
        [Pure, NotNull]
        private LinkedList<FeedforwardResult> CalculateInternal([NotNull] Vector<float> input)
        {
            var layers = _layers;

            // prepare a list of all layer's results
            var feedforwardResults = new LinkedList<FeedforwardResult>();

            // run a forward propagation step and keep track of all intermediate reults
            var nextInput = input;
            foreach (var layer in layers)
            {
                // perform the feedforward iteration
                var layerInput = nextInput;
                var layerResult = layer.Feedforward(layerInput);
                nextInput = layerResult.Output;

                // the input layer must never change the input values,
                // it is here only for cosmetics.
                Debug.Assert((layer.Type != LayerType.Input) || layerResult.Output.Equals(layerInput), "(layer.Type != LayerType.Input) || layerResult.Output.Equals(layerInput)");

                // store the intermediate result
                feedforwardResults.AddLast(layerResult);
            }
            return feedforwardResults;
        }

        /// <summary>
        /// Trains the network using the given <paramref name="trainingSet" />.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="lambda">The regularization parameter; a value of <see literal="0" /> means no regularization.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Regularization parameter must be nonnegative</exception>
        /// <exception cref="System.NotFiniteNumberException">Regularization parameter must be a finite number</exception>
        public void Train([NotNull] IReadOnlyCollection<TrainingExample> trainingSet, int maximumIterations = 1000, float lambda = 0.0F)
        {
            if (lambda < 0) throw new ArgumentOutOfRangeException("lambda", lambda, "Regularization parameter must be nonnegative");
            if (double.IsInfinity(lambda) || double.IsNaN(lambda)) throw new NotFiniteNumberException("Regularization parameter must be a finite number", lambda);

            var trainingResult = lambda > 0
                ? CalculateCostAndGradientRegularized(trainingSet, lambda)
                : CalculateCostAndGradientUnregularized(trainingSet);

            UpdateLayerWeights(trainingResult);

            throw new NotImplementedException("Parameter optimization is not yet implemented.");
        }

        /// <summary>
        /// Updates the layer weights according to the <paramref name="trainingResult" />
        /// </summary>
        /// <param name="trainingResult">The training result.</param>
        /// <param name="learningRate">The learning rate.</param>
        /// <param name="momentum">The descent momentum.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void UpdateLayerWeights(TrainingResult trainingResult, float learningRate = 0.05F, float momentum = 0.8F)
        {

        }

        /// <summary>
        /// Calculates the network's training cost.
        /// </summary>
        /// <param name="expectedOutput">The expected output, i.e. ground truth.</param>
        /// <param name="networkOutput">The network output.</param>
        /// <returns>System.Single.</returns>
        private float CalculateCost(Vector<float> expectedOutput, FeedforwardResult networkOutput)
        {
            Debug.Assert(networkOutput.LayerType == LayerType.Output, "networkOutput.LayerType == LayerType.Output");
            return CalculateCost(expectedOutput, networkOutput.Output);
        }

        /// <summary>
        /// Calculates the network's training cost.
        /// </summary>
        /// <param name="expectedOutput">The expected output, i.e. ground truth.</param>
        /// <param name="networkOutput">The network output.</param>
        /// <returns>System.Single.</returns>
        private float CalculateCost(Vector<float> expectedOutput, Vector<float> networkOutput)
        {
            var logOutput = networkOutput.Map(v => (float)Math.Log(v));
            var firstPart = expectedOutput*logOutput;

            var logInvOutput = networkOutput.Map(v => (float)Math.Log(1 - v));
            var secondPart = (1-expectedOutput) * logInvOutput;

            return -firstPart - secondPart;
        }

        /// <summary>
        /// Calculates the cost given the training examples.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="lambda">The regularization parameter.</param>
        /// <returns>System.Single.</returns>
        /// <exception cref="NotImplementedException"></exception>
        [Pure]
        private TrainingResult CalculateCostAndGradientRegularized([NotNull] IReadOnlyCollection<TrainingExample> trainingSet, float lambda)
        {
            var unregularizedResult = CalculateCostAndGradientUnregularized(trainingSet);
            var count = trainingSet.Count;

            // regularize the training cost
            var layers = _layers.Skip(1);
            var sumOfSquaredWeights = layers.AsParallel().Sum(layer => layer.Weights.Map(v => v*v).RowSums().Sum());
            var costRegularization = lambda/(2*count)*sumOfSquaredWeights;
            var regularizedCost = unregularizedResult.Cost + costRegularization;

            // regularization of gradients is obtained by simply adding
            // the scaled weights to the gradient
            var unregularizedGradients = unregularizedResult.ErrorGradients;
            var regularizationFactor = lambda/count;
            var regularizedGradient = unregularizedGradients.AsParallel().ToDictionary(
                entry => entry.Key,
                entry => RegularizeErrorGradient(entry.Key, entry.Value, regularizationFactor));

            // return the regularized result
            return new TrainingResult(regularizedCost, regularizedGradient);
        }

        /// <summary>
        /// Regularizes the error gradient.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="unregularizedGradient">The unregularized gradient.</param>
        /// <param name="regularizationFactor">The regularization factor (lambda/no. of training examples).</param>
        /// <returns>ErrorGradient.</returns>
        private static ErrorGradient RegularizeErrorGradient([NotNull] Layer layer, ErrorGradient unregularizedGradient, float regularizationFactor)
        {
            var regularizedWeightGradient = unregularizedGradient.Weight + regularizationFactor * layer.Weights;
            return new ErrorGradient(regularizedWeightGradient, unregularizedGradient.Bias);
        }

        /// <summary>
        /// Calculates the cost given the training examples.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <returns>System.Single.</returns>
        /// <exception cref="NotImplementedException"></exception>
        [Pure]
        private TrainingResult CalculateCostAndGradientUnregularized([NotNull] IReadOnlyCollection<TrainingExample> trainingSet)
        {
            // Map: Apply training method to each example
            var trainingResults = trainingSet.Select(CalculateCostAndGradientUnregularized);

            // initialize cost and accumulated gradients
            var gradientDictionary = new Dictionary<Layer, ErrorGradient>();
            var cost = 0.0F;

            // for each layer after the input, initialize the gradient
            // accumulation matrices to zero
            foreach (var layer in _layers.Skip(1))
            {
                Debug.Assert(layer.Type != LayerType.Input, "layer.Type != LayerType.Input");

                var gradient = ErrorGradient.EmptyFromLayer(layer);
                gradientDictionary.Add(layer, gradient);
            }

            // Reduce: merge the training examples
            foreach (var trainingResult in trainingResults)
            {
                // accumulate cost over all training examples
                cost += trainingResult.Cost;

                // iterate over all layer's error gradients of this training example
                // TODO: this is data parallel with respect to layers, so may run in parallel
                var trainingGradients = trainingResult.ErrorGradients;
                foreach (var trainingGradient in trainingGradients)
                {
                    var layer = trainingGradient.Key;
                    var gradient = trainingGradient.Value;

                    // accumulate gradients over all training examples
                    gradientDictionary[layer] += gradient;
                }
            }

            // scale cost and gradients by the number of training examples
            var inverseExampleCount = 1.0F/trainingSet.Count;
            cost *= inverseExampleCount;
            gradientDictionary = gradientDictionary.AsParallel()
                .ToDictionary(item => item.Key, item => item.Value*inverseExampleCount);

            return new TrainingResult(cost, gradientDictionary);
        }

        /// <summary>
        /// Calculates the (unregularized) cost and gradient given a single training example.
        /// </summary>
        /// <param name="example">The training example.</param>
        [Pure]
        private TrainingResult CalculateCostAndGradientUnregularized(TrainingExample example)
        {
            var layers = _layers;

            // prepare the output structures
            var gradients = new Dictionary<Layer, ErrorGradient>();

            // fetch the training data
            var input = example.GetInputs();
            var expectedOutput = example.GetOutputs();

            // perform a feed-forward pass and retrieve the intermediate results
            var feedforwardResults = CalculateInternal(input);
            Debug.Assert(layers.Count == feedforwardResults.Count, "layers.Count == feedforwardResults.Count");

            // we start with the output layer
            var layer = OutputLayer;
            var resultNode = feedforwardResults.Last;

            // Calculate the error of the network output regarding the wanted training output,
            // as well as the error gradient of the output layer.
            // The calculated error will also serve as an input to the layer loop below.
            var error = CalculateNetworkOutputError(feedforwardResults, expectedOutput);
            var outputGradient = CalculateErrorGradient(resultNode, error);
            gradients.Add(layer, outputGradient);

            // calculate the training cost
            var j = CalculateCost(expectedOutput, resultNode.Value);

            // as long as we do not hit the input layer, iterate backwards
            // through all hidden layers
            Debug.Assert(layer.Previous != null, "layer.Previous != null");
            while (layer.Previous.Type != LayerType.Input)
            {
                layer = layer.Previous;
                resultNode = resultNode.Previous;

                // I know it's true, you know it's true ...
                Debug.Assert(layer != null, "layer != null");
                Debug.Assert(resultNode != null, "resultNode != null");

                // obtain this layer's feedforward results
                var feedforwardResult = resultNode.Value;

                // errors on the hidden layer must be obtained through backpropagation
                var delta = layer.Backpropagate(
                    feeforwardResult: feedforwardResult,
                    outputErrors: error);

                // calculate the error gradient
                var hiddenGradient = CalculateErrorGradient(resultNode, delta);
                gradients.Add(layer, hiddenGradient);

                // store the error for the next iteration
                error = delta.WeightErrors;
            }

            return new TrainingResult(j, gradients);
        }

        /// <summary>
        /// Calculates the error gradient of a given layer.
        /// </summary>
        /// <remarks>
        /// This assumes a single training pass. If the gradients are aggregated over multiple training examples,
        /// the result needs to be scaled accordingly.
        /// </remarks>
        /// <param name="resultNode">The layer's feedforward result node.</param>
        /// <param name="error">The layer's output error.</param>
        /// <returns>ErrorGradient.</returns>
        [Pure]
        private static ErrorGradient CalculateErrorGradient([NotNull] LinkedListNode<FeedforwardResult> resultNode, BackpropagationResult error)
        {
            return CalculateErrorGradient(resultNode, error.WeightErrors);
        }

        /// <summary>
        /// Calculates the error gradient of a given layer.
        /// </summary>
        /// <remarks>
        /// This assumes a single training pass. If the gradients are aggregated over multiple training examples,
        /// the result needs to be scaled accordingly.
        /// </remarks>
        /// <param name="resultNode">The layer's feedforward result node.</param>
        /// <param name="weightErrors">The layer's output error.</param>
        /// <returns>ErrorGradient.</returns>
        [Pure]
        private static ErrorGradient CalculateErrorGradient([NotNull] LinkedListNode<FeedforwardResult> resultNode, [NotNull] Vector<float> weightErrors)
        {
            // in order to calculate the gradient, we require the
            // output activations of the previous layer. In case the previous
            // layer happens to be the input layer, this will be the raw inputs.
            var layerInput = GetLayerInput(resultNode);

            // calculate the gradient
            var weightGradient = weightErrors.OuterProduct(layerInput);
            var biasGradient = weightErrors; // the bias input is always 1, so this is trivial

            return new ErrorGradient(weightGradient, biasGradient);
        }

        /// <summary>
        /// Gets the input values of the <see cref="Layer"/> that belongs to the <paramref name="resultNode"/>.
        /// </summary>
        /// <param name="resultNode">The result node.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        [Pure, NotNull]
        private static Vector<float> GetLayerInput([NotNull] LinkedListNode<FeedforwardResult> resultNode)
        {
            Debug.Assert(resultNode.Value.LayerType != LayerType.Input, "resultNode.Value.LayerType != LayerType.Input");

            // obtain the previous node
            var previousLayerNode = resultNode.Previous;
            Debug.Assert(previousLayerNode != null, "previousLayerNode != null");

            // the previous layer's output are the inputs of the layer given to this function
            var previousLayer = previousLayerNode.Value;
            return previousLayer.Output;
        }

        /// <summary>
        /// Calculates the network output error.
        /// </summary>
        /// <param name="feedforwardResults">The feedforward results.</param>
        /// <param name="expectedOutput">The expected output.</param>
        /// <returns>Vector&lt;System.Single&gt;.</returns>
        [Pure, NotNull]
        private static Vector<float> CalculateNetworkOutputError([NotNull] LinkedList<FeedforwardResult> feedforwardResults, [NotNull] Vector<float> expectedOutput)
        {
            var outputLayer = feedforwardResults.Last.Value;
            var networkOutput = outputLayer.Output;
            var error = networkOutput - expectedOutput;
            return error;
        }
    }
}
