using System.Collections.Generic;
using JetBrains.Annotations;

namespace Neural.perceptron
{
    /// <summary>
    /// A network layer consisting of multiple <see cref="Perceptron"/> instances.
    /// </summary>
    class Layer
    {
        /// <summary>
        /// The perceptrons in this layer
        /// </summary>
        [NotNull]
        private readonly IReadOnlyCollection<Perceptron> _perceptrons;

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return _perceptrons.Count; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Layer"/> class.
        /// </summary>
        /// <param name="perceptrons">The perceptrons.</param>
        public Layer([NotNull] IReadOnlyCollection<Perceptron> perceptrons)
        {
            _perceptrons = perceptrons;
        }
    }
}
