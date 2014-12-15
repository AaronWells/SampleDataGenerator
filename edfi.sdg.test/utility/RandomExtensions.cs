using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace edfi.sdg.test.utility
{
    using edfi.sdg.utility;

    [TestClass]
    public class RandomExtensions
    {
        [TestMethod]
        public void NextNormal()
        {
            var r = new Random();
            var values = new int[41];
            const int Sample = 1 << 20;

            const double Mu = 0.0;
            const double Sigma = 1.0;

            for (var i = 0; i < Sample; i++)
            {
                var val = r.NextNormal(Mu, Sigma);
                var idx = (int)Math.Round(val * 10) + 21;
                if (idx >= values.GetLowerBound(0) && idx <= values.GetUpperBound(0)) values[idx]++;
            }

            foreach (var value in values)
            {
                Console.WriteLine(new string('*', value / (Sample >> 10)));
            }
        }

        [TestMethod]
        public void NextChi()
        {
            var r = new Random();
            var values = new int[20];
            const double Sigma = 1.0;
            const int Sample = 1 << 20;
            for (var i = 0; i < Sample; i++)
            {
                var val = r.NextChi(1, Sigma);
                var idx = (int)Math.Round(val * 10);
                if (idx >= values.GetLowerBound(0) && idx <= values.GetUpperBound(0)) values[idx]++;
            }
            foreach (var value in values)
            {
                Console.WriteLine(new string('*', value / (Sample >> 9)));
            }
        }

        [TestMethod]
        public void NextChiSquare()
        {
            var r = new Random();
            var values = new int[20];
            const double Sigma = 1.0;
            const int Sample = 1 << 20;
            for (var i = 0; i < Sample; i++)
            {
                var val = r.NextChiSquare(1, Sigma);
                var idx = (int)Math.Round(val * 10);
                if (idx >= values.GetLowerBound(0) && idx <= values.GetUpperBound(0)) values[idx]++;
            }
            foreach (var value in values)
            {
                Console.WriteLine(new string('*', value / (Sample >> 8)));
            }
        }
    }
}
