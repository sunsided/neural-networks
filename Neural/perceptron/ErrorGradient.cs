using System;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace Widemeadows.MachineLearning.Neural.Perceptron
{
    /// <summary>
    /// Struct ErrorGradient
    /// </summary>
    public struct ErrorGradient : IEquatable<ErrorGradient>
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
        /// Creates a new empty error gradient from the given <paramref name="layer"/>.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns>ErrorGradient.</returns>
        public static ErrorGradient EmptyFromLayer([NotNull] Layer layer)
        {
            var weight = Layer.EmptyWeightFromLayer(layer);
            var bias = Layer.EmptyBiasFromLayer(layer);

            return new ErrorGradient(weight, bias);
        }

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

        /// <summary>
        /// Implements the binary + operator.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static ErrorGradient operator +(ErrorGradient a, ErrorGradient b)
        {
            var weight = a.Weight.Add(b.Weight);
            var bias = a.Bias.Add(b.Bias);
            return new ErrorGradient(weight, bias);
        }

        /// <summary>
        /// Implements the * operator.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="scale">The scale.</param>
        /// <returns>The result of the operator.</returns>
        public static ErrorGradient operator *(ErrorGradient a, float scale)
        {
            var weight = a.Weight * scale;
            var bias = a.Bias * scale;
            return new ErrorGradient(weight, bias);
        }

        /// <summary>
        /// Implements the / operator.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="scale">The scale.</param>
        /// <returns>The result of the operator.</returns>
        public static ErrorGradient operator /(ErrorGradient a, float scale)
        {
            var invScale = 1.0F/scale;
            return a*scale;
        }
    }
}
