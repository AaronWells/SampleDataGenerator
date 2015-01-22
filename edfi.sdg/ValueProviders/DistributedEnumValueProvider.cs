using System;
using System.Linq;
using EdFi.SampleDataGenerator.Distributions;
using EdFi.SampleDataGenerator.Quantity;

namespace EdFi.SampleDataGenerator.ValueProviders
{
    public class DistributedEnumValueProvider<T> : ValueProvider where T : struct, IConvertible
    {
        public DistributionBase Distribution { get; set; }
        
        public QuantityBase Quantity { get; set; }

        public DistributedEnumValueProvider()
        {
            Distribution = new RangeDistribution(); 
            Quantity = new ConstantQuantity { Quantity = 1 };
        }
        
        public override object GetValue(object[] dependsOn)
        {
            if (typeof(T).IsArray)
            {
                return Distribution.Shuffled<T>().Take(Quantity.Next()).ToArray();
            }
            return Distribution.Next<T>();
        }
    }
}
