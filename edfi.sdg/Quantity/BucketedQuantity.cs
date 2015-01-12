using System;
using EdFi.SampleDataGenerator.Distributions;

namespace EdFi.SampleDataGenerator.Quantity
{
    [Serializable]
    public class BucketedQuantity : QuantityBase
    {
        public Weighting[] Weightings { get; set; }

        public BucketedQuantity()
        {
            Weightings = new Weighting[1];
            Weightings[0] = new Weighting { Value = default(int), Weight = 1.0 };
        }

        public override int Next()
        {
            var r = Rand.NextDouble();
            foreach (var item in Weightings)
            {
                if (r <= item.Weight) return (int)item.Value;
                r -= item.Weight;
            }
            throw new IndexOutOfRangeException("Empty Weightings list");
        }
    }
}