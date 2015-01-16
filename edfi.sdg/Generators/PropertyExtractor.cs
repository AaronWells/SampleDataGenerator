using System;
using System.Collections.Generic;
using System.Linq;

namespace EdFi.SampleDataGenerator.Generators
{
    public static class PropertyExtractor
    {
        public static IEnumerable<PropertyMetadata> ExtractPropertyMetadata(Type type)
        {
            var result = new List<PropertyMetadata>();
            var metadata = new PropertyMetadata(type);
            RecursiveGetProperties(type, metadata, new PropertyPath[] { }, result);
            return result;
        }

        private static void RecursiveGetProperties(Type type, PropertyMetadata parent, PropertyPath[] paths, ICollection<PropertyMetadata> propertyMetadata)
        {
            var properties = type.GetProperties().Where(p => p.CanRead && p.CanWrite);

            foreach (var propinfo in properties)
            {
                var metadata = new PropertyMetadata(type, parent, propinfo, paths);
                propertyMetadata.Add(metadata);
                RecursiveGetProperties(propinfo.PropertyType, metadata, metadata.PropertyPaths, propertyMetadata);
            }
        }
    }
}
