using System.Xml.Serialization;

namespace EdFi.SampleDataGenerator.ValueProviders
{
    public class SampleValueProvider : ValueProvider
    {
        [XmlAttribute]
        public string MyValue { get; set; }

        public override object GetValue(params string[] dependsOn)
        {
            return MyValue;
        }
    }
}