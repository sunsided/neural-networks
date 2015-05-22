using System;
using JetBrains.Annotations;

namespace Neural.perceptron
{
    /// <summary>
    /// A perceptron.
    /// </summary>
    class Perceptron
    {
        /// <summary>
        /// The activation function
        /// </summary>
        [NotNull]
        private readonly Activation _activation;

        /// <summary>
        /// Initializes a new instance of the <see cref="Perceptron"/> class.
        /// </summary>
        /// <param name="activation">The activation function.</param>
        public Perceptron([NotNull] Activation activation)
        {
            _activation = activation;
        }

        /// <summary>
        /// Calculates the activation of the perceptron.
        /// </summary>
        /// <returns>System.Single.</returns>
        public float Activate()
        {
            throw new NotImplementedException();
        }
    }
}
