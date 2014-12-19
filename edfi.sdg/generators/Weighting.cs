namespace edfi.sdg.generators
{
    using System;
    using System.Xml.Serialization;

    [System.SerializableAttribute()]
    public class Weighting
    {
        [XmlElement]
        public object Value { get; set; }
        [XmlAttribute]
        public Double Weight { get; set; }
    }
}