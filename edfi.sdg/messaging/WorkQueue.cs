using System;
using System.Messaging;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;

namespace EdFi.SampleDataGenerator.Messaging
{
    public class WorkQueue : IQueueReader, IQueueWriter, IDisposable
    {
        private readonly MessageQueue _queue;
        private readonly MessageQueueTransaction _transaction;

        public WorkQueue(string queueName)
        {
            EnsureQueueExists(queueName);
            _queue = new MessageQueue(queueName) { Formatter = new BinaryMessageFormatter() };
            _transaction = new MessageQueueTransaction();
            _transaction.Begin();
        }

        private static void EnsureQueueExists(string queueName)
        {
            if (!MessageQueue.Exists(queueName))
            {
                MessageQueue.Create(queueName, true);
            }
        }

        public void Dispose()
        {
            if (_transaction.Status == MessageQueueTransactionStatus.Pending)
                _transaction.Commit();
            _transaction.Dispose();
            _queue.Dispose();
        }

        public void WriteObject(object obj)
        {
            var msg = new Message(obj, new BinaryMessageFormatter(FormatterAssemblyStyle.Simple, FormatterTypeStyle.TypesWhenNeeded));
            _queue.Send(msg, _transaction);
        }

        public Task<object> ReadObjectAsync()
        {
            var t = new TaskCompletionSource<object>();
            ReceiveCompletedEventHandler callback = (source, asyncResult) =>
                {
                    try
                    {
                        var q = (MessageQueue)source;
                        var msg = q.EndReceive(asyncResult.AsyncResult);
                        t.TrySetResult(msg.Body);
                    }
                    catch (MessageQueueException ex)
                    {
                        switch (ex.MessageQueueErrorCode)
                        {
                            case MessageQueueErrorCode.IOTimeout:
                                t.TrySetCanceled();
                                break;
                            default:
                                t.TrySetException(ex);
                                break;
                        }
                    }
                };
            _queue.ReceiveCompleted -= callback;
            _queue.ReceiveCompleted += callback;
            _queue.BeginReceive(TimeSpan.FromSeconds(1));
            return t.Task;
        }
    }
}
