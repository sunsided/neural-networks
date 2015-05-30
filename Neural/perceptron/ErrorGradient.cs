using System;
using MathNet.Numerics.LinearAlgebra;

namespace Neural.Perceptron
{
    /// <summary>
    /// Struct ErrorGradient
    /// </summary>
    struct ErrorGradient : IEquatable<ErrorGradient>
    {
        /// <summary>
        /// The weight gradient
        /// </summary>
        public readonly Matrix<float> Weight;

        /// <summary>
        /// The bias error gradient
        /// </summary>
        public readonly Vector<float> Bias;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorGradient"/> struct.
        /// </summary>
        /// <param name="weight">The weight.</param>
        /// <param name="bias">The bias.</param>
        public ErrorGradient(Matrix<float> weight, Vector<float> bias)
        {
            Weight = weight;
            Bias = bias;
        }

        /// <summary>
        /// Determines whether the specified <see cref="ErrorGradient" /> is equal to this instance.
        /// </summary>
        /// <param name="other">Another object to compare to.</param>
        /// <returns><see langword="true" /> if the specified <see cref="ErrorGradient" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
        public bool Equals(ErrorGradient other)
        {
            return Equals(Weight, other.Weight) && Equals(Bias, other.Bias);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><see langword="true" /> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ErrorGradient && Equals((ErrorGradient) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Weight != null ? Weight.GetHashCode() : 0)*397) ^ (Bias != null ? Bias.GetHashCode() : 0);
            }
        }
    }
}
