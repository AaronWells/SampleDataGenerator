using System.Reflection;
using EdFi.SampleDataGenerator.Utility;

namespace EdFi.SampleDataGenerator.Generators.StandardGenerators
{
    public class SystemDataType : IStandardGenerator
    {
        public bool CanHandle(PropertyInfo property)
        {
            return property.PropertyType.IsSystemType();
        }

        public object Handle(PropertyInfo property)
        {
            return CanHandle(property)
                ? property.PropertyType.GetDefaultValue()
                : new ArrayTypeGenerator().Handle(property);
        }
    }
}