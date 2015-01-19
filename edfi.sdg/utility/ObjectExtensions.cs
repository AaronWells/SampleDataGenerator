using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace EdFi.SampleDataGenerator.Utility
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

        public static string ExcludeFirstSegment(this string propertyName)
        {
            var breakDown = propertyName.Split(new[] { '.' });
            return breakDown.Length < 2 ? propertyName : string.Join(".", breakDown.Skip(1));
        }

        /// <summary>
        /// finds and returns the relative object from the input paramenter based on the provided path
        /// </summary>
        /// <param name="input">any object as the root of a hierarchy</param>
        /// <param name="propertyChain">property specifier from the root of hierarchy</param>
        public static object LocateObject(this object input, string propertyChain)
        {
            if (string.IsNullOrWhiteSpace(propertyChain)) return null; // this is error condition

            if (!propertyChain.Contains("."))
                return input;

            var segmentObject = input.GetPropertyValue(propertyChain.FirstSegment());
            return LocateObject(segmentObject, propertyChain.ExcludeFirstSegment());
        }

        public static object Clone(this object obj)
        {
            var serializer = new DataContractSerializer(obj.GetType());
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                return serializer.ReadObject(ms);
            }
        }
    }
}