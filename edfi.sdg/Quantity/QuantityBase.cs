using System;
using System.Xml.Serialization;

namespace EdFi.SampleDataGenerator.Quantity
{
    [Serializable]
    [XmlInclude(typeof(ConstantQuantity))]
    [XmlInclude(typeof(RangeQuantity))]
    [XmlInclude(typeof(NormalQuantity))]
    [XmlInclude(typeof(ChiQuantity))]
    [XmlInclude(typeof(ChiSquareQuantity))]
    [XmlInclude(typeof(BucketedQuantity))]
    public abstract class QuantityBase : IQuantity
    {
        protected static readonly Random Rand = new Random();
        public abstract int Next();
    }
}
