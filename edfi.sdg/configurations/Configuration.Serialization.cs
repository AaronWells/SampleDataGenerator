using System;
using System.Collections.Generic;
using System.Linq;

namespace edfi.sdg.configurations
{
    using System.Reflection;
    using System.Xml.Serialization;

    public partial class Configuration
    {
        public static XmlSerializer ConfigurationSerializer()
        {
            var knownTypes = new List<Type>();

            var properties = Assembly.GetAssembly(typeof(AssemblyLocator)).GetTypes().SelectMany(x => x.GetProperties())
                    .Where(p => p.GetCustomAttribute<GenericXmlElementAttribute>() != null);

            foreach (var propertyInfo in properties)
            {
                var attrib = propertyInfo.GetCustomAttribute<GenericXmlElementAttribute>();
                knownTypes.AddRange(attrib.GetKnownTypes(propertyInfo));
            }

            return new XmlSerializer(typeof(Configuration), knownTypes.Distinct().ToArray());
        }
    }
}
