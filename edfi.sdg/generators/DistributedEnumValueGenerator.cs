namespace edfi.sdg.generators
{
    using System;
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
            var current = Weightings.First().Value;
            var r = Rnd.NextDouble();
            foreach (var item in Weightings)
            {
                current = item.Value;
                if (r >= item.Weight)
                {
                    r -= item.Weight;
                }
                else break;
            }
            var type = input.GetType();
            type.GetProperty(Property).GetSetMethod().Invoke(input, new object[] { current });
            
            if (type.GetProperty(Property + "Specified") != null)
                type.GetProperty(Property + "Specified").GetSetMethod().Invoke(input, new object[] { true });

            return new[] { input };
        }
    }
}
