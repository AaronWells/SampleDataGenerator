using System;
using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Configurations;
using EdFi.SampleDataGenerator.Models;

namespace EdFi.SampleDataGenerator.ValueProviders
{
    [Serializable]
    public class ValueRule
    {
        [XmlAttribute]
        public string Class { get; set; }

        [XmlAttribute]
        public string PropertySpecifier { get; set; }

        [GenericXmlElement(BaseTargetType = typeof(ComplexObjectType))]
        public ValueProvider ValueProvider { get; set; }

        [XmlIgnore]
        public bool HasDependency { get { return ValueProvider != null && ValueProvider.HasDependency; } }

        [XmlIgnore]
        public string Path
        {
            get { return string.Format("{0}::{1}", Class, PropertySpecifier); }
        }
    }
}
