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

        public abstract object GetValue(string[] lookupPropertyValues);
    }

    public class SampleValueProvider : ValueProvider
    {
        [XmlAttribute]
        public string MyValue { get; set; }

        public override object GetValue(string[] lookupPropertyValues)
        {
            return MyValue;
        }
    }
}
