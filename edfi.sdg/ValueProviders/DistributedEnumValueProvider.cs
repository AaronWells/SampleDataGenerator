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
    public class DistributedEnumValueProvider<T> : ValueProvider
        where T : struct, IConvertible
    {
        public DistributionBase Distribution { get; set; }
        
        public QuantityBase Quantity { get; set; }

        public DistributedEnumValueProvider()
        {
            Distribution = new RangeDistribution(); 
            Quantity = new ConstantQuantity { Quantity = 1 };
        }
        
        public override object GetValue(params string[] dependsOn)
        {
            if (typeof(T).IsArray)
            {
                return Distribution.Shuffled<T>().Take(Quantity.Next()).ToArray();
            }
            return Distribution.Next<T>();
        }
    }

    [Obsolete]
    [Serializable]
    public class DistributedEnumWorkItem<T> : WorkItem where T : struct, IConvertible
    {
        [XmlAttribute]
        public string Property { get; set; }

        public DistributionBase Distribution { get; set; }
        
        public QuantityBase Quantity { get; set; }

        public DistributedEnumWorkItem()
        {
            Distribution = new RangeDistribution(); // RangeDistribution<T>();
            Quantity = new ConstantQuantity { Quantity = 1 };
        }

        protected override object[] DoWorkImplementation(object input, IConfiguration configuration)
        {
            var type = input.GetType();

            if (Property.FirstSegment() != type.Name) return new[] {input};

            var propertyName = Property.LastSegment();
            var property = type.GetProperty(propertyName);

            if (property != null)
            {
                if (property.PropertyType.IsArray)
                {
                    var array = Distribution.Shuffled<T>().Take(Quantity.Next()).ToArray();
                    property.GetSetMethod().Invoke(input, new object[] { array });
                }
                else
                {
                    var single = Distribution.Next<T>();
                    property.GetSetMethod().Invoke(input, new object[] { single });
                    if (type.GetProperty(propertyName + "Specified") != null)
                    {
                        type.GetProperty(propertyName + "Specified")
                            .GetSetMethod()
                            .Invoke(input, new object[] { true });
                    }
                }
            }
            return new[] { input };
        }
    }
}
