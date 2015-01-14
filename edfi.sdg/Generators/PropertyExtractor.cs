using System;
using System.Collections.Generic;
using System.Linq;

namespace EdFi.SampleDataGenerator.Generators
{
    using System.Reflection;

    public struct PropertyPath
    {
        private readonly Type type;

        private readonly string[] path;

        public PropertyPath(Type type, IEnumerable<string> path)
            : this()
        {
            this.type = type;
            this.path = path.ToArray();
        }

        public Type Type { get { return this.type; } }

        public IEnumerable<string> Path { get { return this.path; } }

        public override string ToString()
        {
            return string.Format("{0}::{1}", type.Name, string.Join(".", Path));
        }
    }

    public class PropertyMetadata : IComparable<PropertyMetadata>, IComparable<string>
    {
        private readonly PropertyPath[] propertyPaths;

        private readonly PropertyInfo propertyInfo;

        private readonly Type type;

        private readonly PropertyMetadata parentPropertyMetadata;

        public PropertyPath[] PropertyPaths { get { return this.propertyPaths; } }

        public PropertyInfo PropertyInfo { get { return this.propertyInfo; } }

        public Type Type { get { return this.type; } }

        public PropertyMetadata ParentPropertyMetadata { get { return this.parentPropertyMetadata; } }

        public PropertyMetadata(Type type)
        {
            this.type = type;
            this.propertyInfo = null;
            this.propertyPaths = new PropertyPath[] { };
        }

        public PropertyMetadata(Type parentType, PropertyMetadata parentPropertyMetadata, PropertyInfo propertyInfo, IEnumerable<PropertyPath> parentPropertyPaths)
        {
            this.type = parentType;
            this.parentPropertyMetadata = parentPropertyMetadata;
            this.propertyInfo = propertyInfo;

            var paths = new List<PropertyPath>() { new PropertyPath(parentType, new String[] { propertyInfo.Name }) };
            paths.AddRange(
                from parentPropertyPath in parentPropertyPaths
                let propNames = new List<string>(parentPropertyPath.Path) { propertyInfo.Name }
                select new PropertyPath(parentPropertyPath.Type, propNames)
                );

            this.propertyPaths = paths.ToArray();
        }

        public override string ToString()
        {
            return string.Join("~", this.PropertyPaths.Select(x => x.ToString()));
        }

        public int CompareTo(PropertyMetadata other)
        {
            return System.String.Compare(this.ToString(), other.ToString(), System.StringComparison.Ordinal);
        }

        public int CompareTo(string other)
        {
            var paths = this.PropertyPaths.Select(p => p.ToString());
            //todo: match a this propertyMedatadata to a string path that includes parent.parent. navigation, 
            // other.replace("parent", this.parentProperty.propertyname... or something
            return paths.Any(x => System.String.Compare(x, other, System.StringComparison.Ordinal) == 0) ? 0 : 1;
        }
    }

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
