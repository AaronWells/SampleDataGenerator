namespace edfi.sdg.interfaces
{
    using System.Threading.Tasks;

    public interface IQueueReader
    {
        Task<object> ReadObjectAsync();
    }
}