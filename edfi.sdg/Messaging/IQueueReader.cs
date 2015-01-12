using System.Threading.Tasks;

namespace EdFi.SampleDataGenerator.Messaging
{
    public interface IQueueReader
    {
        Task<object> ReadObjectAsync();
    }
}