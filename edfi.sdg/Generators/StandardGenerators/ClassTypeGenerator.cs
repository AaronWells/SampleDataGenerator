using System;
using System.Reflection;
using EdFi.SampleDataGenerator.Utility;

namespace EdFi.SampleDataGenerator.Generators.StandardGenerators
{
    public class ClassTypeGenerator : IStandardGenerator
    {
        public bool CanHandle(PropertyInfo property)
        {
            return property.PropertyType.IsCompositeType();
        }

        public object Handle(PropertyInfo property)
        {
            return CanHandle(property)
                ? Activator.CreateInstance(property.PropertyType)
                : new NullValueGenerator().Handle(property);
        }
    }
}