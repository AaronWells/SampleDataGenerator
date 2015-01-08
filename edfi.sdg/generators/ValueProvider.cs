using System;

namespace edfi.sdg.generators
{
    using System.Xml.Serialization;

    [Serializable]
    [XmlInclude(typeof(SampleValueProvider))]
    public abstract class ValueProvider
    {
        public abstract object GetValue();
        public abstract object[] GetValues();
    }

    public class SampleValueProvider : ValueProvider
    {
        [XmlAttribute]
        public string MyValue { get; set; }

        public override object GetValue()
        {
            return MyValue;
        }

        public override object[] GetValues()
        {
            return new object[] { MyValue + "1", MyValue + "2" };
        }
    }
}
