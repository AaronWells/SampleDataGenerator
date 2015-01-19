using System;
using System.Linq;
using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Configurations;
using EdFi.SampleDataGenerator.Distributions;
using EdFi.SampleDataGenerator.Quantity;
using EdFi.SampleDataGenerator.Utility;
using EdFi.SampleDataGenerator.WorkItems;

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
        
        public override object GetValue(object[] dependsOn =  null)
        {
            if (typeof(T).IsArray)
            {
                return Distribution.Shuffled<T>().Take(Quantity.Next()).ToArray();
            }
            return Distribution.Next<T>();
        }
    }
}
