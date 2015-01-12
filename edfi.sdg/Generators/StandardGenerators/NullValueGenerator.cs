using System.Reflection;

namespace EdFi.SampleDataGenerator.Generators.StandardGenerators
{
    public class NullValueGenerator : IStandardGenerator
    {
        public bool CanHandle(PropertyInfo property)
        {
            return true;
        }

        public object Handle(PropertyInfo property)
        {
            return null;
        }
    }
}