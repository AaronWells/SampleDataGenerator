using System;

namespace edfi.sdg.generators
{
    using System.Xml.Serialization;

    using edfi.sdg.interfaces;
    using edfi.sdg.utility;

    [Serializable]
    public abstract class Quantity: IQuantity
    {
        protected static readonly Random Rnd = new Random();
        public abstract int Next();
    }

    [Serializable]
    public class ConstantQuantity : Quantity
    {
        [XmlAttribute]
        public int Quantity { get; set; }
        public override int Next()
        {
            return Quantity;
        }
    }

    [Serializable]
    public class RangeQuantity : Quantity
    {
        [XmlAttribute]
        public int Min { get; set; }
        [XmlAttribute]
        public int Max { get; set; }
        public override int Next()
        {
            return Rnd.Next(this.Min, this.Max);
        }
    }

    [Serializable]
    public class NormalQuantity : RangeQuantity
    {
        public override int Next()
        {
            var mu = (Min + Max) / 2;
            var sigma = Math.Abs(Max - Min) / 6.0;
            var result = (int)Math.Round(Rnd.NextNormal(mu, sigma));
            if (result < Min) return Min;
            return result > this.Max ? this.Max : result;
        }
    }

    [Serializable]
    public class ChiQuantity : RangeQuantity
    {
        public override int Next()
        {
            var sigma = Math.Abs(Max - Min) / 3.0;
            var result = (int)Math.Round(Rnd.NextChi(1, sigma));
            if (result < Min) return Min;
            return result > this.Max ? this.Max : result;
        }
    }

    [Serializable]
    public class ChiSquareQuantity : RangeQuantity
    {
        public override int Next()
        {
            var sigma = Math.Abs(Max - Min) / 3.0;
            var result = (int)Math.Round(Rnd.NextChiSquare(1, sigma));
            if (result < Min) return Min;
            return result > this.Max ? this.Max : result;
        }
    }


    [Serializable]
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
