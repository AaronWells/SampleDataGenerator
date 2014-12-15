namespace edfi.sdg.interfaces
{
    public interface IGenerator
    {
        int Id { get; set; }

        void Generate(object input, IQueueWriter queueWriter, IConfiguration configuration);
    }
}
