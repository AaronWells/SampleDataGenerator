using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace edfi.sdg.test.messaging
{
    using edfi.sdg.interfaces;
    using edfi.sdg.test.classes;

    [TestClass]
    public class WorkQueue
    {
        private const string WorkQueueName = @".\Private$\edfi.sdg.test.messaging";

        [TestMethod]
        public void ReadWriteTest()
        {
            var obj1 = new SerializableTestClass();

            using (var writer = new edfi.sdg.messaging.WorkQueue(WorkQueueName))
            {
                (writer as IQueueWriter).WriteObject(obj1);
            }

            using (var reader = new edfi.sdg.messaging.WorkQueue(WorkQueueName))
            {
                var task = (reader as IQueueReader).ReadObjectAsync();
                task.Wait();
                var obj2 = (SerializableTestClass)task.Result;
                Assert.AreEqual(obj1.Name, obj2.Name);
            }
        }
    }
}
