using System.Collections.Generic;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace Widemeadows.MachineLearning.Neural.Perceptron
{
    /// <summary>
    /// Interface IFeedforward
    /// </summary>
    public interface IFeedforward
    {
        /// <summary>
        /// Performs a feed-forward pass through the network and stores the results of each layer.
        /// </summary>
        /// <param name="input">The inputs.</param>
        /// <returns>LinkedList&lt;Layer.FeedforwardResult&gt;.</returns>
        LinkedList<FeedforwardResult> Feedforward([NotNull] Vector<float> input);
    }
}