using System;

namespace edfi.sdg.configurations
{
    using System.Xml.Serialization;

    using edfi.sdg.generators;
    using edfi.sdg.models;

    [Serializable]
    public class ValueRule
    {
        [XmlAttribute]
        public string Criteria { get; set; }

        [GenericXmlElementAttribute(BaseTargetType = typeof(ComplexObjectType))]
        public ValueProvider ValueProvider { get; set; }
    }
}
