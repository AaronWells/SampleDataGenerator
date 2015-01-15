using System;
using System.Linq;
using System.Xml.Serialization;

namespace EdFi.SampleDataGenerator.ValueProvider
{
    [Serializable]
    public abstract class ValueProvider
    {
        [XmlArray]
        public string[] LookupProperties { get; set; }

        public abstract object GetValue(params string[] lookupPropertyValues);

        [XmlIgnore]
        public bool HasDependency { get { return LookupProperties != null && LookupProperties.Any(); } }
    }
}
