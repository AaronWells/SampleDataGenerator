using System;

namespace edfi.sdg.generators
{
    using System.Configuration;
    using System.IO;
    using System.Linq;

    public abstract class Quantity
    {
        protected static readonly Random Rnd = new Random();
        public abstract int Next();
    }

    public class ConstantQuantity : Quantity
    {
        public int Quantity { get; set; }
        public override int Next()
        {
            return Quantity;
        }
    }

    public class RangeQuantity : Quantity
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public override int Next()
        {
            return Rnd.Next((int)Min, (int)Max);
        }
    }

    public class LinearQuantity : RangeQuantity
    {
        public override int Next()
        {
            return base.Next();
        }
    }

    public class NormalQuantity : RangeQuantity
    {
        public override int Next()
        {
            return base.Next();
        }
    }

    public class BucketedQuantity : Quantity
    {
        public Weighting<int>[] Weightings { get; set; }

        public BucketedQuantity()
        {
            Weightings = new Weighting<int>[1];
            Weightings[0] = new Weighting<int> { Value = default(int), Weight = 1.0 };
        }

        public override int Next()
        {
            var r = Rnd.NextDouble();
            foreach (var item in Weightings)
            {
                if (r <= item.Weight) return item.Value;
                r -= item.Weight;
            }
            throw new IndexOutOfRangeException("Empty Weightings list");
        }
    }
}
