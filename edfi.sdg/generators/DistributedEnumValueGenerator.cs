namespace edfi.sdg.generators
{
    using System;
    using System.Linq;

    using edfi.sdg.interfaces;
    using edfi.sdg.messaging;

    [System.SerializableAttribute()]
    public class DistributedEnumValueGenerator<T> : Generator
    {
        public DistributedEnumValue<T>[] Distribution { get; set; }

        public string PropertyName { get; set; }

        public DistributedEnumValueGenerator()
        {
            var i = 0;
            var values = (T[])Enum.GetValues(typeof(T));
            Distribution = new DistributedEnumValue<T>[values.Length];
            foreach (var value in values)
            {
                Distribution[i++] = new DistributedEnumValue<T> { Value = value, Weight = 1.0 / values.Length };
            }
        }

        public override void Generate(object input, IQueueWriter queueWriter, IConfiguration configuration)
        {
            var current = Distribution.First().Value;
            var r = Rnd.NextDouble();
            foreach (var item in Distribution)
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
