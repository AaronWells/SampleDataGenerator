namespace EdFi.SampleDataGenerator.Writers
{
    using System;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Serialization;

    using EdFi.SampleDataGenerator.Data;
    using EdFi.SampleDataGenerator.Models;

    public static class InterchangeWriter
    {
        public static void Write<TInterchange, TItems>(DataRepository repository, XmlWriter writer) where TInterchange : IInterchange<TItems>, new()
        {
            var interchange = new TInterchange();
            var serializer = new XmlSerializer(typeof(TInterchange));
            var tmpArray = new TItems[] { };

            var attribs = typeof(TInterchange).GetTypeInfo().GetDeclaredProperty("Items").GetCustomAttributes<XmlElementAttribute>();
            foreach (var xmlElementAttribute in attribs)
            {
                var oldLength = tmpArray.Length;
                var items = repository.GetByClassName<TItems>(xmlElementAttribute.Type.FullName);
                var newLength = oldLength + items.Length;
                Array.Resize(ref tmpArray, newLength);
                items.CopyTo(tmpArray, oldLength);
            }
            interchange.Items = tmpArray;
            serializer.Serialize(writer, interchange);
        }
    }
}
