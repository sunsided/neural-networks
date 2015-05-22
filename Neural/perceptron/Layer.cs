using System.Collections.Generic;

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
        private readonly ICollection<Perceptron> _perceptrons;

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
        public Layer(ICollection<Perceptron> perceptrons)
        {
            _perceptrons = perceptrons;
        }
    }
}
