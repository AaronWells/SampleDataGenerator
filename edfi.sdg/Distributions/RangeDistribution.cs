using System;
using System.Linq;
using EdFi.SampleDataGenerator.Utility;

namespace EdFi.SampleDataGenerator.Distributions
{
    [Serializable]
    public class RangeDistribution : DistributionBase
    {
        public override T Next<T>()
        {
            var values = Enum.GetValues(typeof(T));
            return Rand.NextArray((T[])values);
        }

        public override T[] Shuffled<T>()
        {
            var values = (T[])Enum.GetValues(typeof(T));
            return values.Select(x => new { order = Rand.Next(), value = x })
                .OrderBy(x => x.order)
                .Select(x => x.value)
                .ToArray();
        }
    }
}