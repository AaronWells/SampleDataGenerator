using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EdFi.SampleDataGenerator.Utility
{
    public static class TypeExtensions
    {
        public static bool IsSystemType(this Type type)
        {
            return type.Namespace == "System";
        }

        public static bool IsCompositeType(this Type type)
        {
            return type.IsClass && !type.IsArray && type.Namespace != "System";
        }

        public static IEnumerable<PropertyInfo> GetSystemProperties(this Type type)
        {
            return type.GetProperties().Where(p => p.PropertyType.Namespace == "System");
        }

        public static IEnumerable<PropertyInfo> GetCompositeProperties(this Type type)
        {
            return type.GetProperties().Where(
                p => !p.PropertyType.IsArray && p.PropertyType.Namespace != "System" && p.PropertyType.IsClass);
        }

        public static bool IsAssociation(this Type t)
        {
            return t != null && t.Name.EndsWith("Association");
        }
    }
}