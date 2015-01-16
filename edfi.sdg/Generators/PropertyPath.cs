using System;
using System.Collections.Generic;
using System.Linq;

namespace EdFi.SampleDataGenerator.Generators
{
    public struct PropertyPath
    {
        private readonly Type _type;

        private readonly string[] _pathSegment;

        public PropertyPath(Type type, IEnumerable<string> path)
            : this()
        {
            _type = type;
            _pathSegment = path.ToArray();
        }

        public Type Type { get { return _type; } }

        public IEnumerable<string> PathSegment { get { return _pathSegment; } }

        public override string ToString()
        {
            return string.Format("{0}::{1}", _type.Name, PropertyChain);
        }

        public string PropertyChain
        {
            get { return string.Join(".", PathSegment); }
        }
    }
}