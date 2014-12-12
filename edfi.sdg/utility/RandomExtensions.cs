using System;

namespace edfi.sdg.utility
{
    /// <summary>
    /// Extension methods to the Random class.
    /// Taken from Practical Numerical Methods with C# by Jack Xu
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Returns a random value that conforms to a standard deviation curve
        /// </summary>
        /// <param name="rnd">Random number generaror</param>
        /// <param name="mu">Average value of population</param>
        /// <param name="sigma">Value of one standard deviation</param>
        /// <returns>the next random number in the sequence</returns>
        public static double NextNormal(this Random rnd, double mu, double sigma)
        {
            double v1 = 0.0, v2 = 0.0, v12 = 0.0, y = 0.0;
            while (v12 >= 1.0 || v12.Equals(0.0))
            {
                v1 = 2.0 * rnd.NextDouble() - 1.0;
                v2 = 2.0 * rnd.NextDouble() - 1.0;
                v12 = v1 * v1 + v2 * v2;
            }
            y = Math.Sqrt(-1.0 * Math.Log(v12) / v12);
            return v1 * y * sigma + mu;
        }

        /// <summary>
        /// Returns a random number corresponding to the Chi distribution
        /// </summary>
        /// <param name="rnd">Random number generaror</param>
        /// <param name="n">Degrees of freedom</param>
        /// <param name="sigma">Value of one standard deviation</param>
        /// <returns>the next random number in the sequence</returns>
        public static double NextChi(this Random rnd, int n, double sigma)
        {
            return Math.Sqrt(NextChiSquare(rnd, n, sigma) / n);
        }

        /// <summary>
        /// Returns a random number corresponding to the Chi squared distribution
        /// </summary>
        /// <param name="rnd">Random number generaror</param>
        /// <param name="n">Degrees of freedom</param>
        /// <param name="sigma">Value of one standard deviation</param>
        /// <returns>the next random number in the sequence</returns>
        public static double NextChiSquare(this Random rnd, int n, double sigma)
        {
            if (n <= 0)
            {
                throw new ArgumentOutOfRangeException("n", n, "n must be positive.");
            }
            if (sigma <= 0.0)
            {
                throw new ArgumentOutOfRangeException("sigma", sigma, "sigma must be positive");
            }
            var result = 0.0;
            for (var i = 0; i < n; i++)
            {
                result += Math.Pow(NextNormal(rnd, 0, sigma * sigma), 2);
            }
            return result;
        }
    }
}
