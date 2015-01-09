using System;

namespace edfi.sdg.generators
{
    using System.Xml.Serialization;

    using edfi.sdg.interfaces;

    [Serializable]
    public abstract class ValueProvider: IValueProvider
    {
        public abstract object GetValue();
    }

    public class SampleValueProvider : ValueProvider
    {
        [XmlAttribute]
        public string MyValue { get; set; }

        public override object GetValue()
        {
            return MyValue;
        }
    }
}
