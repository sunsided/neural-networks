using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Widemeadows.MachineLearning.Neural.Demonstration.Digit
{
    public static class Extensions
    {
        /// <summary>
        /// Wholes the population standard dev.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>System.Double.</returns>
        public static double WholePopulationStdDev(this ICollection<double> values, double average)
        {
            double ret = 0;
            int count = values.Count();
            if (count > 1)
            {
                //Perform the Sum of (value-avg)^2
                double sum = values.Sum(d => (d - average) * (d - average));

                //Put it all together
                ret = Math.Sqrt(sum / count);
            }
            return ret;
        }
    }
}
