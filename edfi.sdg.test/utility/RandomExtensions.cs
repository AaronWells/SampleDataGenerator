using System;
using EdFi.SampleDataGenerator.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdFi.SampleDataGenerator.Test.Utility
{
    [TestClass]
    public class RandomExtensions
    {
        [TestMethod]
        public void NextNormal()
        {
            var r = new Random();
            var values = new int[41];
            const int sample = 1 << 20;

            const double mu = 0.0;
            const double sigma = 1.0;

            for (var i = 0; i < sample; i++)
            {
                var val = r.NextNormal(mu, sigma);
                var idx = (int)Math.Round(val * 10) + 21;
                if (idx >= values.GetLowerBound(0) && idx <= values.GetUpperBound(0)) values[idx]++;
            }

            foreach (var value in values)
            {
                Console.WriteLine(new string('*', value / (sample >> 10)));
            }
        }

        [TestMethod]
        public void NextChi()
        {
            var r = new Random();
            var values = new int[20];
            const double sigma = 1.0;
            const int sample = 1 << 20;
            for (var i = 0; i < sample; i++)
            {
                var val = r.NextChi(1, sigma);
                var idx = (int)Math.Round(val * 10);
                if (idx >= values.GetLowerBound(0) && idx <= values.GetUpperBound(0)) values[idx]++;
            }
            foreach (var value in values)
            {
                Console.WriteLine(new string('*', value / (sample >> 9)));
            }
        }

        [TestMethod]
        public void NextChiSquare()
        {
            var r = new Random();
            var values = new int[20];
            const double sigma = 1.0;
            const int sample = 1 << 20;
            for (var i = 0; i < sample; i++)
            {
                var val = r.NextChiSquare(1, sigma);
                var idx = (int)Math.Round(val * 10);
                if (idx >= values.GetLowerBound(0) && idx <= values.GetUpperBound(0)) values[idx]++;
            }
            foreach (var value in values)
            {
                Console.WriteLine(new string('*', value / (sample >> 8)));
            }
        }

        [TestMethod]
        public void NextArray()
        {
            var r = new Random();
            var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var values = new int[array.Length];
            const int sample = 1 << 20;
            for (var i = 0; i < sample; i++)
            {
                var val = r.NextArray(array);
                values[val]++;
            }
            foreach (var idx in array)
            {
                Console.Write(idx + ": ");
                Console.WriteLine(new string('*', values[idx] / (sample >> 8)));
            }
        }

        [TestMethod]
        public void NextWeighted()
        {
            var r = new Random();
            var weights = new[] { 0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0};
            var values = new int[weights.Length];
            const int sample = 1 << 20;
            for (var i = 0; i < sample; i++)
            {
                var idx = r.NextWeighted(weights);
                values[idx]++;
            }
            for (var i = weights.GetLowerBound(0); i <= weights.GetUpperBound(0); i++)
            {
                Console.Write(weights[i] + ": ");
                Console.WriteLine(new string('*', values[i] / (sample >> 8)));
            }
        }
    }
}
