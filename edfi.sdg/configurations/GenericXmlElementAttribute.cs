using System;

namespace edfi.sdg.configurations
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;

    using edfi.sdg.utility;

    [AttributeUsage(AttributeTargets.Property)]
    public class GenericXmlElementAttribute : Attribute
    {
        public Type BaseTargetType { get; set; }

        /// <summary>
        /// Create a concrete list of attribute overrides for all possible generics
        /// </summary>
        /// <returns></returns>
        public XmlAttributes GetXmlAttributes(PropertyInfo propertyInfo)
        {
            var result = new XmlAttributes();

            var types = Assembly.GetExecutingAssembly().GetTypes();

            var propertyType = propertyInfo.PropertyType.IsGenericType
                                    ? propertyInfo.PropertyType.GetGenericTypeDefinition()
                                    : propertyInfo.PropertyType;

            propertyType = propertyType.IsArray ? propertyType.GetElementType() : propertyType;

            var derivedTypes = types.Where(t => t.IsSubClassOfGeneric(propertyType)).ToArray();

            var enumTypes = types.Where(t => t.IsEnum).ToArray();
            var modelTypes = types.Where(t => BaseTargetType.IsAssignableFrom(t) && !t.IsAbstract).ToArray();

            foreach (var type in derivedTypes)
            {
                if (type.IsGenericType)
                {
                    var constraints = type.GetGenericArguments().SelectMany(x => x.GetGenericParameterConstraints()).ToArray();

                    var isEnum =
                        constraints.All(y => y == typeof(ValueType) || y == typeof(IConvertible));

                    foreach (var enumType in isEnum ? enumTypes : modelTypes)
                    {
                        Type[] typeArgs = { enumType };
                        var constructedType = type.MakeGenericType(typeArgs);
                        var attr = new XmlElementAttribute(enumType.Name + type.Name, constructedType);
                        result.XmlElements.Add(attr);
                    }
                }
                else
                {
                    var attr = new XmlElementAttribute(type.Name, type);
                    result.XmlElements.Add(attr);
                }
            }

            return result;
        }

        public IEnumerable<Type> GetKnownTypes(PropertyInfo propertyInfo)
        {
            var result = new List<Type>();

            var types = Assembly.GetExecutingAssembly().GetTypes();

            var propertyType = propertyInfo.PropertyType.IsGenericType
                                    ? propertyInfo.PropertyType.GetGenericTypeDefinition()
                                    : propertyInfo.PropertyType;

            propertyType = propertyType.IsArray ? propertyType.GetElementType() : propertyType;

            var derivedTypes = types.Where(t => t.IsSubClassOfGeneric(propertyType)).ToArray();

            var enumTypes = types.Where(t => t.IsEnum).ToArray();
            var modelTypes = types.Where(t => BaseTargetType.IsAssignableFrom(t) && !t.IsAbstract).ToArray();

            foreach (var type in derivedTypes)
            {
                if (type.IsGenericType)
                {
                    var constraints = type.GetGenericArguments().SelectMany(x => x.GetGenericParameterConstraints()).ToArray();
                    var isEnum = constraints.All(y => y == typeof(ValueType) || y == typeof(IConvertible));
                    result.AddRange(from enumType in isEnum ? enumTypes : modelTypes select type.MakeGenericType(new Type[] { enumType }));
                }
                else
                {
                    result.Add(type);
                }
            }

            return result;
        }

    }
}
