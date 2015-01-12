using System;
using System.Xml.Serialization;

namespace EdFi.SampleDataGenerator.Distributions
{
    [Serializable]
    public class Weighting
    {
        [XmlElement]
        public object Value { get; set; }
        [XmlAttribute]
        public Double Weight { get; set; }
    }
}