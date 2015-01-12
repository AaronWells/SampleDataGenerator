using System;
using System.Xml.Serialization;

namespace EdFi.SampleDataGenerator.Quantity
{
    [Serializable]
    public class RangeQuantity : QuantityBase
    {
        [XmlAttribute]
        public int Min { get; set; }
        [XmlAttribute]
        public int Max { get; set; }
        public override int Next()
        {
            return Rand.Next(Min, Max);
        }
    }
}