namespace edfi.sdg.generators
{
    using System;
    using System.Linq;

    using edfi.sdg.interfaces;
    using edfi.sdg.messaging;

    [System.SerializableAttribute()]
    public class DistributedEnumValueGenerator<T> : Generator where T: struct, IConvertible
    {
        public Weighting<T>[] Weightings { get; set; }

        public string PropertyName { get; set; }

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

        public override void Generate(object input, IQueueWriter queueWriter, IConfiguration configuration)
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
            type.GetProperty(PropertyName).GetSetMethod().Invoke(input, new object[] { current });
            queueWriter.WriteObject(new WorkEnvelope { NextStep = Id, Model = input });
        }
    }
}
