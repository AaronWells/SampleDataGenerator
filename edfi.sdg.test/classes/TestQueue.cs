using System.Collections.Concurrent;
using System.Threading.Tasks;
using EdFi.SampleDataGenerator.Messaging;

namespace EdFi.SampleDataGenerator.Test.Classes
{
    /// <summary>
    /// Thread-safe, non-transactional, local, memory-based worker queue for testing purposes
    /// </summary>
    internal class TestQueue : IQueueReader, IQueueWriter
    {
        private readonly ConcurrentQueue<object> _objects = new ConcurrentQueue<object>();// Stack();

        /// <summary>
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _objects.Count == 0;
            }
        }

        /// <summary>
        /// </summary>
        public void WriteObject(object obj)
        {
            _objects.Enqueue(obj);
        }

        /// <summary>
        /// </summary>
        public Task<object> ReadObjectAsync()
        {
            object result;
            var t = new TaskCompletionSource<object>();
            t.TrySetResult(_objects.TryDequeue(out result) ? result : null);
            return t.Task;
        }
    }
}
