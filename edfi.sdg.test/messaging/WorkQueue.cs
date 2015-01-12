using EdFi.SampleDataGenerator.Messaging;
using EdFi.SampleDataGenerator.Test.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdFi.SampleDataGenerator.Test.Messaging
{
    [TestClass]
    public class WorkQueue
    {
        private const string WorkQueueName = @".\Private$\edfi.sdg.test.messaging";

        [TestMethod]
        public void ReadWriteTest()
        {
            var obj1 = new SerializableTestClass();

            using (var writer = new SampleDataGenerator.Messaging.WorkQueue(WorkQueueName))
            {
                (writer as IQueueWriter).WriteObject(obj1);
            }

            using (var reader = new SampleDataGenerator.Messaging.WorkQueue(WorkQueueName))
            {
                var task = (reader as IQueueReader).ReadObjectAsync();
                task.Wait();
                var obj2 = (SerializableTestClass)task.Result;
                Assert.AreEqual(obj1.id, obj2.id);
            }
        }
    }
}
