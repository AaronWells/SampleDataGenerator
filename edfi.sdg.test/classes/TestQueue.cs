using EdFi.SampleDataGenerator.Messaging;

namespace edfi.sdg.test.classes
{
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    /// <summary>
    /// Thread-safe, non-transactional, local, memory-based worker queue for testing purposes
    /// </summary>
    internal class TestQueue : IQueueReader, IQueueWriter
    {
        private readonly ConcurrentQueue<object> _objects = new ConcurrentQueue<object>();// Stack();

        public bool IsEmpty
        {
            get
            {
                return _objects.Count == 0;
            }
        }

        public void WriteObject(object obj)
        {
            _objects.Enqueue(obj);
        }

        public Task<object> ReadObjectAsync()
        {
            object result;
            var t = new TaskCompletionSource<object>();
            t.TrySetResult(this._objects.TryDequeue(out result) ? result : null);
            return t.Task;
        }
    }
}
