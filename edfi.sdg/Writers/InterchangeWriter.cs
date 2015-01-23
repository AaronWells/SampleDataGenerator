namespace EdFi.SampleDataGenerator.Writers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using EdFi.SampleDataGenerator.Data;
    using EdFi.SampleDataGenerator.Models;

    public static class InterchangeWriter
    {
        public static void WriteInterchanges(string path, Type[] interchangeTypes)
        {
            var repository = new DataRepository();
            foreach (var interchangeType in interchangeTypes)
            {
                var itemsType = GetItemsType(interchangeType);
                var fullpath = Path.ChangeExtension(Path.Combine(path, interchangeType.Name), "xml");
                var writer = new XmlTextWriter(fullpath, Encoding.UTF8);
                var method = typeof(InterchangeWriter).GetMethod("Write");
                var generic = method.MakeGenericMethod(interchangeType, itemsType);
                generic.Invoke(null, new object[] { repository, writer });
            }
        }

        public static Type GetItemsType(Type interchangeType)
        {
            var interchangeInterface = interchangeType.GetInterfaces()
                .Single(x => x.FullName.StartsWith("EdFi.SampleDataGenerator.Models.IInterchange") && x.IsGenericType);
            return interchangeInterface.GetGenericArguments().Single();
        }

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
