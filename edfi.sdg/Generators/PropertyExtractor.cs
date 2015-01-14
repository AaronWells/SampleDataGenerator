using System;
using System.Collections.Generic;
using System.Linq;

namespace EdFi.SampleDataGenerator.Generators
{
    using System.Reflection;

    public class PropertyMetadata : IComparable<PropertyMetadata>
    {
        public string PropertyPath { get; private set; }

        public PropertyInfo PropertyInfo { get; private set; }

        public PropertyMetadata(string propertyPath, PropertyInfo propertyInfo)
        {
            PropertyPath = propertyPath;
            PropertyInfo = propertyInfo;
        }

        public int CompareTo(PropertyMetadata other)
        {
            return System.String.Compare(PropertyPath, other.PropertyPath, System.StringComparison.Ordinal);
        }
    }

    public static class PropertyExtractor
    {
        public static IEnumerable<PropertyMetadata> ExtractPropertyMetadata(Type type)
        {
            var result = new List<PropertyMetadata>();
            RecursiveGetProperties(type, null, result);
            return result;
        }

        private static void RecursiveGetProperties(Type type, string parentPath, ICollection<PropertyMetadata> propertyMetadata)
        {
            var properties = type.GetProperties().Where(p => p.CanRead && p.CanWrite);

            foreach (var propinfo in properties)
            {
                var path = string.IsNullOrEmpty(parentPath) ? propinfo.Name : String.Join(".", new string[] { parentPath, propinfo.Name });
                propertyMetadata.Add(new PropertyMetadata(path, propinfo));
                RecursiveGetProperties(propinfo.PropertyType, path, propertyMetadata);
            }
        }
    }
}
