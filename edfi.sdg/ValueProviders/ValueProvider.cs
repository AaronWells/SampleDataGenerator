using System;
using System.Linq;
using System.Xml.Serialization;

namespace EdFi.SampleDataGenerator.ValueProviders
{
    [Serializable]
    public abstract class ValueProvider
    {
        protected ValueProvider()
        {
            LookupProperties = new string[0];
        }

        [XmlArray]
        public string[] LookupProperties { get; set; }

        public abstract object GetValue(object[] dependsOn);

        [XmlIgnore]
        public bool HasDependency { get { return LookupProperties != null && LookupProperties.Any(); } }
    }
}
