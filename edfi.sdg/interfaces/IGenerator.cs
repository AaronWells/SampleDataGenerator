namespace edfi.sdg.interfaces
{
    public interface IGenerator
    {
        int Id { get; set; }

        object[] Generate(object input, IConfiguration configuration);
    }
}
