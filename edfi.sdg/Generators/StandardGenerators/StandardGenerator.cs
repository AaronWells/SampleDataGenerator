using System.Reflection;

namespace EdFi.SampleDataGenerator.Generators.StandardGenerators
{
    public class StandardGenerator : IStandardGenerator
    {
        public bool CanHandle(PropertyInfo property)
        {
            return false;
        }

        public object Handle(PropertyInfo property)
        {
            return new SystemDataType().Handle(property);
        }
    }
}