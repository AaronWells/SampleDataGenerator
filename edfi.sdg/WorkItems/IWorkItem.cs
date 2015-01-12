using EdFi.SampleDataGenerator.Configurations;

namespace EdFi.SampleDataGenerator.WorkItems
{
    public interface IWorkItem
    {
        int Id { get; set; }

        object[] DoWork(object input, IConfiguration configuration);
    }
}
