using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Widemeadows.MachineLearning.Neural.Demonstration.Digit
{
    /// <summary>
    /// Class Extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Shuffles the specified list.
        /// </summary>
        /// <remarks>
        /// Fisher-Yates algorithm as per http://stackoverflow.com/a/1262619/195651
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <returns>IList&lt;T&gt;.</returns>
        [NotNull]
        public static IList<T> Shuffle<T>([NotNull] this IReadOnlyCollection<T> input)
        {
            Random rng = new Random();

            var list = new List<T>(input);

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }
}
