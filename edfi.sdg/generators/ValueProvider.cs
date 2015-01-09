using System;

namespace edfi.sdg.generators
{
    using System.Xml.Serialization;

    using edfi.sdg.interfaces;

    [Serializable]
    public abstract class ValueProvider : IValueProvider
    {
        [XmlArray]
        public string[] LookupProperties { get; set; }

        public abstract object GetValue();

        public abstract object GetValue(params string[] lookupPropertyValues);
    }

    public class SampleValueProvider : ValueProvider
    {
        [XmlAttribute]
        public string MyValue { get; set; }

        public override object GetValue()
        {
            return this.GetValue(string.Empty);
        }

        public override object GetValue(params string[] lookupPropertyValues)
        {
            return MyValue;
        }
    }
}
