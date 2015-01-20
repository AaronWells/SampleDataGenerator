using System.IO;
using System.Xml.Serialization;

namespace EdFi.SampleDataGenerator.Models
{
    public interface IComplexObjectType
    {
        // ReSharper disable once InconsistentNaming
        string id { get; set; }
    }

    public static class IComplexObjectTypeExtensions
    {
        public static string ToXml(this IComplexObjectType obj)
        {
            var serializer = new XmlSerializer(obj.GetType());
            var textWriter = new StringWriter();
            serializer.Serialize(textWriter, obj);
            return textWriter.ToString();
        }

    }
}
