using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace edfi.sdg.test.utility
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using edfi.sdg.utility;

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

            for (int i = 0; i < sample; i++)
            {
                var val = r.NextNormal(0, 1.0);
                var idx = (int)Math.Round(val * 10) + 21;
                if (idx >= values.GetLowerBound(0) && idx <= values.GetUpperBound(0))
                    Interlocked.Increment(ref values[idx]);
            }

            foreach (var value in values)
            {
                Console.WriteLine(new string('*', value / (sample >> 10)));
            }
        }


    }
}
