using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using EdFi.SampleDataGenerator.ValueProviders;

namespace EdFi.SampleDataGenerator.Generators
{
    public class PropertyMetadata : IComparable
    {
        private readonly PropertyPath[] _propertyPaths;

        private readonly PropertyInfo _propertyInfo;

        private readonly Type _type;

        private readonly PropertyMetadata _parentPropertyMetadata;

        public PropertyPath[] PropertyPaths { get { return _propertyPaths; } }

        public PropertyInfo PropertyInfo { get { return _propertyInfo; } }

        public Type Type { get { return _type; } }

        public PropertyMetadata ParentPropertyMetadata { get { return _parentPropertyMetadata; } }

        public PropertyPath AbsolutePath { get { return _propertyPaths.Last(); } }

        public ValueRule BestMatchingRule { get; set; }

        public PropertyMetadata(Type type)
        {
            _type = type;
            _propertyInfo = null;
            _propertyPaths = new[] { new PropertyPath(_type, new string[] { }), };
        }

        public PropertyMetadata(Type parentType, PropertyMetadata parentPropertyMetadata, PropertyInfo propertyInfo, IEnumerable<PropertyPath> parentPropertyPaths)
        {
            _type = parentType;
            _parentPropertyMetadata = parentPropertyMetadata;
            _propertyInfo = propertyInfo;

            var paths = new List<PropertyPath> { new PropertyPath(parentType, new[] { propertyInfo.Name }) };
            paths.AddRange(
                from parentPropertyPath in parentPropertyPaths
                let propNames = new List<string>(parentPropertyPath.PathSegment) { propertyInfo.Name }
                select new PropertyPath(parentPropertyPath.Type, propNames)
                );

            _propertyPaths = paths.ToArray();
        }

        public override string ToString()
        {
            return string.Join("~", PropertyPaths.Select(x => x.ToString()));
        }

        public int CompareTo(object obj)
        {
            var other = obj as PropertyMetadata;
            return CompareTo(other);
        }

        public int CompareTo(PropertyMetadata other)
        {
            return string.Compare(ToString(), other.ToString(), StringComparison.Ordinal);
        }

        public int CompareTo(string other)
        {
            return Matches(other) ? 0 : 1;
        }

        public bool Matches(string path)
        {
            var paths = PropertyPaths.Select(p => p.ToString());
            return paths.Any(x => string.Compare(x, path, StringComparison.Ordinal) == 0);
        }

        public string ResolveRelativePath(string relativePath)
        {
            const string parentMetadata = "{parent}";
            var workingPath = relativePath.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var currentMetadata = _parentPropertyMetadata;
            while (workingPath.First().StartsWith(parentMetadata) && _parentPropertyMetadata != null)
            {
                currentMetadata = currentMetadata.ParentPropertyMetadata;
                workingPath.RemoveAt(0);
            }
            if (workingPath.Any(x => x.StartsWith(parentMetadata)) || currentMetadata == null)
            {
                throw new InvalidExpressionException(string.Format("'{0}' is not a valid relative path for '{1}'", relativePath, PropertyPaths.Last()));
            }
            var paths = new List<string>(currentMetadata._propertyPaths.Last().PathSegment.Concat(workingPath));
            var parentPropPath = new PropertyPath(currentMetadata.Type, paths);
            return parentPropPath.ToString();
        }
    }
}