using System;
using System.Xml.Serialization;

namespace EdFi.SampleDataGenerator.ValueProvider
{
    [Serializable]
    public abstract class ValueProvider
    {
        [XmlArray]
        public string[] LookupProperties { get; set; }

        public abstract object GetValue(params string[] lookupPropertyValues);
    }
}
