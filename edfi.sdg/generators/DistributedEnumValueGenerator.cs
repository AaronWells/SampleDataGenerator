namespace edfi.sdg.generators
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Xml.Serialization;

    using edfi.sdg.interfaces;

    [System.SerializableAttribute()]
    public class DistributedEnumValueGenerator<T> : Generator where T : struct, IConvertible
    {
        public Weighting<T>[] Weightings { get; set; }

        [XmlAttribute]
        public string Property { get; set; }

        public DistributedEnumValueGenerator()
        {
            var i = 0;
            var values = (T[])Enum.GetValues(typeof(T));
            Weightings = new Weighting<T>[values.Length];
            foreach (var value in values)
            {
                Weightings[i++] = new Weighting<T> { Value = value, Weight = 1.0 / values.Length };
            }
        }

        public override object[] Generate(object input, IConfiguration configuration)
        {
            var type = input.GetType();
            if (Property.StartsWith(type.Name))
            {
                var value = this.GetRandomValue();
                var propertyName = Property.Substring(type.Name.Length + 1);
                if (type.GetProperty(propertyName) != null)
                {
                    type.GetProperty(propertyName).GetSetMethod().Invoke(input, new object[] { value });

                    if (type.GetProperty(propertyName + "Specified") != null)
                    {
                        type.GetProperty(propertyName + "Specified").GetSetMethod().Invoke(input, new object[] { true });
                    }
                }
                else
                {
                    throw new ConfigurationErrorsException(string.Format("no property named '{0}' exists on model.", Property));
                }
            }
            return new[] { input };
        }

        public T GetRandomValue()
        {
            var result = Weightings.First().Value;
            var r = Rnd.NextDouble();
            foreach (var item in Weightings)
            {
                result = item.Value;
                if (r >= item.Weight)
                {
                    r -= item.Weight;
                }
                else break;
            }
            return result;
        }
    }
}
