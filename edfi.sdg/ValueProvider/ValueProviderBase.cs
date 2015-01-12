using System;
using System.Xml.Serialization;

namespace EdFi.SampleDataGenerator.ValueProvider
{
    [Serializable]
    public abstract class ValueProviderBase : IValueProvider
    {
        [XmlArray]
        public string[] LookupProperties { get; set; }

        public abstract object GetValue();

        public abstract object GetValue(params string[] lookupPropertyValues);
    }
}
