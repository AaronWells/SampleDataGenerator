using EdFi.SampleDataGenerator.Configurations;
using EdFi.SampleDataGenerator.Generators;

namespace EdFi.SampleDataGenerator.WorkItems
{
    public class PropertyPopulatorWorkItem : WorkItem
    {
        protected override object[] DoWorkImplementation(object input, IConfiguration configuration)
        {
            var generator = new Generator(configuration.GetValueRules());
            generator.Populate(input);
            return new[] { input };
        }
    }
}
