namespace edfi.sdg.test.classes
{
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    using edfi.sdg.interfaces;

    /// <summary>
    /// Thread-safe, non-transactional, local, memory-based worker queue for testing purposes
    /// </summary>
    internal class TestQueue : IQueueReader, IQueueWriter
    {
        private readonly ConcurrentQueue<object> objects = new ConcurrentQueue<object>();// Stack();

        public bool IsEmpty
        {
            get
            {
                return objects.Count == 0;
            }
        }

        public void WriteObject(object obj)
        {
            objects.Enqueue(obj);
        }

        public Task<object> ReadObjectAsync()
        {
            object result;
            var t = new TaskCompletionSource<object>();
            t.TrySetResult(this.objects.TryDequeue(out result) ? result : null);
            return t.Task;
        }
    }
}
