using System;
using System.Reflection;

namespace EdFi.SampleDataGenerator.Generators.StandardGenerators
{
    public class ClassTypeGenerator : IStandardGenerator
    {
        public bool CanHandle(PropertyInfo property)
        {
            return property.PropertyType.IsClass;
        }

        public object Handle(PropertyInfo property)
        {
            return CanHandle(property)
                ? Activator.CreateInstance(property.PropertyType)
                : new NullValueGenerator().Handle(property);
        }
    }
}