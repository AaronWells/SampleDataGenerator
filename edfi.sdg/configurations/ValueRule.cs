using System;
using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Models;

namespace EdFi.SampleDataGenerator.Configurations
{
    [Serializable]
    public class ValueRule
    {
        [XmlAttribute]
        public string Criteria { get; set; }

        [GenericXmlElement(BaseTargetType = typeof(ComplexObjectType))]
        public ValueProvider.ValueProviderBase ValueProvider { get; set; }
    }
}
