using System;
using EdFi.SampleDataGenerator.Utility;

namespace EdFi.SampleDataGenerator.Quantity
{
    [Serializable]
    public class NormalQuantity : RangeQuantity
    {
        public override int Next()
        {
            var mu = (Min + Max) / 2;
            var sigma = Math.Abs(Max - Min) / 6.0;
            var result = (int)Math.Round(Rand.NextNormal(mu, sigma));
            if (result < Min) return Min;
            return result > Max ? Max : result;
        }
    }
}