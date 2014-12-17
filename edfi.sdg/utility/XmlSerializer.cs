using System;
using System.Linq;

namespace edfi.sdg.utility
{
    using System.Reflection;
    using System.Xml.Serialization;

    using edfi.sdg.configurations;

    public static class XmlSerializer
    {
        private static XmlAttributeOverrides overrides = null;

        /// <summary>
        /// Use reflection to gather all the generic types and all possible (known) 
        /// </summary>
        /// <param name="anAssembly">defaults to this assembly</param>
        /// <returns>XmlAttributeOverrides</returns>
        public static XmlAttributeOverrides XmlAttributeOverrides(Assembly anAssembly = null)
        {
            if (overrides == null)
            {
                overrides = new XmlAttributeOverrides();

                var types = (anAssembly ?? Assembly.GetAssembly(typeof(AssemblyLocator))).GetTypes();

                var enumTypes = types.Where(x => x.IsEnum).ToArray();

                // for all serializable classes labeled "SerializableGeneric"
                var serializableTypes =
                    types.Where(x => x.GetCustomAttribute<SerializableAttribute>() != null).ToArray();

                var generictypes = serializableTypes.Where(x => x.IsGenericTypeDefinition).ToArray();

                foreach (var serializabletype in serializableTypes)
                {
                    var attributes =
                        serializabletype.GetProperties()
                            .Where(p => p.GetCustomAttribute<SerializableGenericEnumAttribute>() != null);

                    foreach (var attribute in attributes)
                    {
                        var attrs = new XmlAttributes();
                        var attrName = attribute.Name;

                        if (attribute.PropertyType.IsGenericTypeDefinition)
                        {
                            var genericTypeDef = attribute.PropertyType.GetGenericTypeDefinition();
                            var derivedgenerics = types.Where(t => t.IsSubclassOf(genericTypeDef));
                        }

                        var genericType = attribute.PropertyType.GetGenericTypeDefinition();

                        var derivedTypes = types.Where(t => t.IsSubclassOf(attribute.PropertyType) || attribute.PropertyType.IsAssignableFrom(t)).ToArray();
                        foreach (var derivedtype in derivedTypes)
                        {
                            if (derivedtype.IsGenericTypeDefinition)
                                foreach (var enumType in enumTypes)
                                {
                                    Type[] typeArgs = { enumType };
                                    var constructedType = derivedtype.MakeGenericType(typeArgs);

                                    var attr = new XmlElementAttribute(enumType.Name + attribute.Name, constructedType);

                                    attrs.XmlElements.Add(attr);
                                }
                        }
                        overrides.Add(serializabletype, attribute.Name, attrs);
                    }
                }
            }
            return overrides;
        }
    }
}
