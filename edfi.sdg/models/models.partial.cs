namespace edfi.sdg.models
{
    using System.IO;
    using System.Xml.Serialization;

    using edfi.sdg.interfaces;

    public abstract partial class ComplexObjectType : IComplexObjectType
    {
        public string ToXml()
        {
            var serializer = new XmlSerializer(this.GetType());
            var textWriter = new StringWriter();
            serializer.Serialize(textWriter, this);
            return textWriter.ToString();
        }

        public static ComplexObjectType FromXml(string xml)
        {
            var serializer = new XmlSerializer(typeof(ComplexObjectType));
            var textReader = new StringReader(xml);
            return (ComplexObjectType)serializer.Deserialize(textReader);
        }
    }
}
