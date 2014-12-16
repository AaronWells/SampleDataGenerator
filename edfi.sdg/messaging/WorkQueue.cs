namespace edfi.sdg.messaging
{
    using System;
    using System.Messaging;
    using System.Runtime.Serialization.Formatters;
    using System.Threading.Tasks;

    using edfi.sdg.interfaces;

    public class WorkQueue : IQueueReader, IQueueWriter, IDisposable
    {
        private readonly MessageQueue queue;
        private readonly MessageQueueTransaction transaction;

        public WorkQueue(string queueName)
        {
            EnsureQueueExists(queueName);
            queue = new MessageQueue(queueName) { Formatter = new BinaryMessageFormatter() };
            transaction = new MessageQueueTransaction();
            transaction.Begin();
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
            if (transaction.Status == MessageQueueTransactionStatus.Pending)
                transaction.Commit();
            transaction.Dispose();
            queue.Dispose();
        }

        public void WriteObject(object obj)
        {
            var msg = new Message(obj, new BinaryMessageFormatter(FormatterAssemblyStyle.Simple, FormatterTypeStyle.TypesWhenNeeded));
            queue.Send(msg, transaction);
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
            queue.ReceiveCompleted -= callback;
            queue.ReceiveCompleted += callback;
            queue.BeginReceive(TimeSpan.FromSeconds(1));
            return t.Task;
        }
    }
}
