namespace edfi.sdg.generators
{
    using System;
    using System.Linq;
    using System.Xml.Serialization;

    using edfi.sdg.configurations;
    using edfi.sdg.interfaces;
    using edfi.sdg.models;

    [Serializable]
    public class DistributedEnumValueGenerator<[SerializableGenericEnum]T> : Generator where T : struct, IConvertible
    {
        [XmlAttribute]
        public string Property { get; set; }

        [SerializableGenericEnum]
        [XmlElement("SexTypeRangeDistribution", typeof(RangeDistribution<SexType>))]
        [XmlElement("OldEthnicityTypeRangeDistribution", typeof(RangeDistribution<OldEthnicityType>))]
        public object Distribution { get; set; }
        
        public Quantity Quantity { get; set; }

        public DistributedEnumValueGenerator()
        {
            Distribution = new RangeDistribution<T>();
            Quantity = new ConstantQuantity() { Quantity = 1 };
        }

        public override object[] Generate(object input, IConfiguration configuration)
        {
            var type = input.GetType();

            if (Property.StartsWith(type.Name))
            {
                var propertyName = Property.Substring(type.Name.Length + 1);
                var property = type.GetProperty(propertyName);

                if (property != null)
                {
                    if (property.PropertyType.IsArray)
                    {
                        var array = ((Distribution<T>)Distribution).Shuffled().Take(Quantity.Next()).ToArray();
                        property.GetSetMethod().Invoke(input, new object[] { array });
                    }
                    else
                    {
                        var single = ((Distribution<T>)Distribution).Next();
                        property.GetSetMethod().Invoke(input, new object[] { single });
                        if (type.GetProperty(propertyName + "Specified") != null)
                        {
                            type.GetProperty(propertyName + "Specified")
                                .GetSetMethod()
                                .Invoke(input, new object[] { true });
                        }
                    }
                }
            }
            return new[] { input };
        }
    }
}
