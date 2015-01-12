using System;
using System.Linq;
using EdFi.SampleDataGenerator.Utility;

namespace EdFi.SampleDataGenerator.Distributions
{
    [Serializable]
    public class BucketedDistribution : DistributionBase
    {
        public Weighting[] Weightings { get; set; }

        public override T Next<T>()
        {
            var weights = Weightings.Select(x => x.Weight).ToArray();
            var idx = Rand.NextWeighted(weights);
            return (T)Weightings[idx].Value;
        }

        public override T[] Shuffled<T>()
        {
            return Weightings.Select(x => new { order = Rand.Next() * x.Weight, item = x })
                .OrderByDescending(x => x.order)
                .Select(x => (T)x.item.Value)
                .ToArray();
        }
    }
}