using System;
using EdFi.SampleDataGenerator.Utility;

namespace EdFi.SampleDataGenerator.Quantity
{
    [Serializable]
    public class ChiQuantity : RangeQuantity
    {
        public override int Next()
        {
            var sigma = Math.Abs(Max - Min) / 3.0;
            var result = (int)Math.Round(Rand.NextChi(1, sigma));
            if (result < Min) return Min;
            return result > Max ? Max : result;
        }
    }
}