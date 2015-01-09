using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using edfi.sdg.generators;

namespace edfi.sdg.utility
{
    public static class ObjectExtensions
    {
        public static object GetPropertyValue(this object o, string propertyName)
        {
            var property = o.GetType().GetProperty(propertyName);
            if (property == null) throw new InvalidPropertyException(propertyName);
            return property.GetValue(o);
        }

        public static void SetPropertyValue(this object o, string propertyName, object value)
        {
            var property = o.GetType().GetProperty(propertyName);
            if(property == null) throw new InvalidPropertyException(propertyName);
            property.SetValue(o, value);
        }

        public static object GetDefaultValue(this object o)
        {
            return o.GetType().GetDefaultValue();
        }

        public static object GetDefaultValue(this Type t)
        {
            return t.IsValueType ? Activator.CreateInstance(t) : null;
        }

        public static string FirstSegment(this string propertyName)
        {
            return propertyName.Split(new[] {'.'})[0];
        }

        public static string LastSegment(this string propertyName)
        {
            var splitted = propertyName.Split(new[] {'.'});
            return splitted[splitted.Length - 1];
        }
    }

    public static class TypeExtensions
    {
        private static IEnumerable<PropertyInfo> GetSystemProperties(this Type type)
        {
            return type.GetProperties().Where(p => p.PropertyType.Namespace == "System");
        }

        private static IEnumerable<PropertyInfo> GetCompositeProperties(this Type type)
        {
            return type.GetProperties().Where(
                p => !p.PropertyType.IsArray && p.PropertyType.Namespace != "System" && p.PropertyType.IsClass);
        }

    }
}