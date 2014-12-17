using System;
using System.Linq;

namespace edfi.sdg.generators
{
    using edfi.sdg.utility;

    public abstract class Distribution<T> where T : struct, IConvertible
    {
        // ReSharper disable once StaticFieldInGenericType
        protected static readonly Random Rnd = new Random();

        public abstract T Next();
        public abstract T[] Shuffled();
    }

    public class ConstantDistribution<T> : Distribution<T>
        where T : struct, IConvertible
    {
        public T Value { get; set; }

        public override T Next()
        {
            return Value;
        }

        public override T[] Shuffled()
        {
            return new T[] { Value };
        }
    }

    public class RangeDistribution<T> : Distribution<T> where T : struct, IConvertible
    {
        public override T Next()
        {
            var values = (T[])Enum.GetValues(typeof(T));
            return Rnd.NextArray<T>(values);
        }

        public override T[] Shuffled()
        {
            var values = (T[])Enum.GetValues(typeof(T));
            return values.Select(x => new { order = Rnd.Next(), value = x })
                    .OrderBy(x => x.order)
                    .Select(x => x.value)
                    .ToArray();
        }
    }

    public class BucketedDistribution<T> : Distribution<T>
        where T : struct, IConvertible
    {
        public Weighting<T>[] Weightings { get; set; }

        public override T Next()
        {
            var weights = Weightings.Select(x => x.Weight).ToArray();
            var idx = Rnd.NextWeighted(weights);
            return Weightings[idx].Value;
        }

        public override T[] Shuffled()
        {
            return Weightings.Select(x => new { order = Rnd.Next() * x.Weight, item = x })
                    .OrderByDescending(x => x.order)
                    .Select(x => x.item.Value)
                    .ToArray();
        }
    }
}
