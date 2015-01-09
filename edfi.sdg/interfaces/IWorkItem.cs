namespace edfi.sdg.interfaces
{
    public interface IWorkItem
    {
        int Id { get; set; }

        object[] DoWork(object input, IConfiguration configuration);
    }
}
