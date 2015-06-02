using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;
using Widemeadows.MachineLearning.Neural.Perceptron;

namespace Widemeadows.MachineLearning.Neural.Training.Backpropagation
{
    public interface IBackpropagation
    {
        /// <summary>
        /// Performs a backpropagation step of the layer's <paramref name="outputErrors" />.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="feedforwardResult">The layer's feedforward result.</param>
        /// <param name="outputErrors">The training errors.</param>
        /// <returns>The activations of this layer's perceptrons.</returns>
        /// <exception cref="System.InvalidOperationException">Attempted to backpropagate through the input layer.</exception>
        BackpropagationResult Backpropagate([NotNull] Layer layer, FeedforwardResult feedforwardResult, [NotNull] Vector<float> outputErrors);
    }
}