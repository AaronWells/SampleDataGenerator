namespace EdFi.SampleDataGenerator.Models
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    public static class ComplexObjectTypeExtensions
    {
        public static string ToXml(this IComplexObjectType obj)
        {
            var serializer = new XmlSerializer(obj.GetType());
            var textWriter = new StringWriter();
            serializer.Serialize(textWriter, obj);
            return textWriter.ToString();
        }

        public static IComplexObjectType FromXml(string typename, string xml)
        {
            var type = Type.GetType(typename);
            if (type == null)
            {
                throw new SerializationException(string.Format("{0} is not a known type", typename));
            }
            var serializer = new XmlSerializer(type);
            var textReader = new StringReader(xml);
            return (IComplexObjectType)serializer.Deserialize(textReader);
        }
    }
}