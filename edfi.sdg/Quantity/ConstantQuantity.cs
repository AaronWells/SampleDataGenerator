using System;
using System.Xml.Serialization;

namespace EdFi.SampleDataGenerator.Quantity
{
    [Serializable]
    public class ConstantQuantity : QuantityBase
    {
        [XmlAttribute]
        public int Quantity { get; set; }
        public override int Next()
        {
            return Quantity;
        }
    }
}