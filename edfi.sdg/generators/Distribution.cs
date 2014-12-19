using System;
using System.Linq;

namespace edfi.sdg.generators
{
    using System.Xml.Serialization;

    using edfi.sdg.utility;

    [Serializable]
    [XmlInclude(typeof(ConstantDistribution))]
    [XmlInclude(typeof(RangeDistribution))]
    [XmlInclude(typeof(BucketedDistribution))]
    public abstract class Distribution
    {
        protected static readonly Random Rnd = new Random();
        public abstract T Next<T>();
        public abstract T[] Shuffled<T>();
    }

    [Serializable]
    public class ConstantDistribution : Distribution
    {
        public object Value { get; set; }

        public override T Next<T>()
        {
            return (T)Value;
        }

        public override T[] Shuffled<T>()
        {
            return new T[] { (T)Value };
        }
    }

    [Serializable]
    public class RangeDistribution : Distribution
    {
        public override T Next<T>()
        {
            var values = Enum.GetValues(typeof(T));
            return Rnd.NextArray<T>((T[])values);
        }

        public override T[] Shuffled<T>()
        {
            var values = (T[])Enum.GetValues(typeof(T));
            return values.Select(x => new { order = Rnd.Next(), value = x })
                    .OrderBy(x => x.order)
                    .Select(x => x.value)
                    .ToArray();
        }
    }

    [Serializable]
    public class BucketedDistribution : Distribution
    {
        public Weighting[] Weightings { get; set; }

        public override T Next<T>()
        {
            var weights = Weightings.Select(x => x.Weight).ToArray();
            var idx = Rnd.NextWeighted(weights);
            return (T)Weightings[idx].Value;
        }

        public override T[] Shuffled<T>()
        {
            return Weightings.Select(x => new { order = Rnd.Next() * x.Weight, item = x })
                    .OrderByDescending(x => x.order)
                    .Select(x => (T)x.item.Value)
                    .ToArray();
        }
    }
}
