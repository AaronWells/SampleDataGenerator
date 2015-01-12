using System;
using System.Reflection;

namespace EdFi.SampleDataGenerator.Generators.StandardGenerators
{
    public class ArrayTypeGenerator : IStandardGenerator
    {
        public bool CanHandle(PropertyInfo property)
        {
            return property.PropertyType.IsArray;
        }

        public object Handle(PropertyInfo property)
        {
            // to do: how many element to create and what to do with the array elements?
            return CanHandle(property)
                ? Activator.CreateInstance(property.PropertyType, new object[] { 1 })
                : new ClassTypeGenerator().Handle(property);
        }
    }
}