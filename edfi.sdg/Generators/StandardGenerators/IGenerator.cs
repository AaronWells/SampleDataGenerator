using System.Reflection;

namespace EdFi.SampleDataGenerator.Generators.StandardGenerators
{
    public interface IGenerator
    {
        bool CanHandle(PropertyInfo property);
        object Handle(PropertyInfo property);
    }
    public interface IStandardGenerator : IGenerator { }
    public interface ICustomGenerator : IStandardGenerator { }
}