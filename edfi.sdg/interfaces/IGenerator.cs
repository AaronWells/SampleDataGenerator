namespace edfi.sdg.interfaces
{
    public interface IGenerator
    {
        int Id { get; set; }
        string Name { get; set; }
        void Generate(object input, IQueueWriter queueWriter, IConfiguration configuration);
    }
}
