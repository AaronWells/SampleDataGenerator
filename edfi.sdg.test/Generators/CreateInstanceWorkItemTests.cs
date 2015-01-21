using EdFi.SampleDataGenerator.Configurations;
using EdFi.SampleDataGenerator.Models;
using EdFi.SampleDataGenerator.Quantity;
using EdFi.SampleDataGenerator.Test.Classes;
using EdFi.SampleDataGenerator.WorkItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdFi.SampleDataGenerator.Test.Generators
{
    [TestClass]
    public class CreateInstanceWorkItemTests
    {
        /// <summary>
        /// The <see cref="CreateInstanceWorkItem"/> should distribute work among multiple queued work items, 
        /// and create exactly the specified quantity of items.
        /// </summary>
        [TestMethod]
        public void DistributeWork()
        {
            const int specifiedQuantity = 1000000;

            var generatedQuantity = 0;

            var queue = new TestQueue();

            var configuration = new Configuration
            {
                MaxQueueWrites = 50
            };

            var generator = (WorkItem) new CreateInstanceWorkItem
            {
                Id = 1,
                CreatedType = typeof(SerializableTestClass).FullName,
                QuantitySpecifier = new ConstantQuantity {Quantity = specifiedQuantity},
            };

            foreach (var tmp in generator.DoWork(null, configuration))
            {
                queue.WriteObject(tmp);
            }

            while (!queue.IsEmpty)
            {
                var task = queue.ReadObjectAsync();
                task.Wait();
                var obj = task.Result as WorkItem;
                if (obj == null)
                {
                    generatedQuantity++; //count one item
                }
                else
                {
                    foreach (var tmp in obj.DoWork(null, configuration))
                    {
                        queue.WriteObject(tmp);
                    }
                }
            }
            Assert.AreEqual(specifiedQuantity, generatedQuantity);
        }

        [TestClass]
        public class AssociationCreationTest
        {
            public class A : IComplexObjectType
            {
                public string id { get; set; }
            }

            public class B : IComplexObjectType
            {
                public string id { get; set; }
            }

            public class ABAssociation
            {
                public AReferenceType AReference { get; set; }
                public BReferenceType BReference { get; set; }
            }

            public class ReferenceType
            {
                public string id { get; set; }
            }

            public class AReferenceType : ReferenceType
            {
            }

            public class BReferenceType : ReferenceType
            {
            }

            [TestMethod]
            public void SimpleAssociation()
            {
                var input = new A {id = "aid"};
                var workItem = new CreateInstanceWorkItem
                {
                    Id = 1,
                    CreatedType = typeof (ABAssociation).FullName,
                    QuantitySpecifier = new ConstantQuantity {Quantity = 1}
                };
                var result = workItem.DoWork(input, new Configuration{MaxQueueWrites = int.MaxValue});
                Assert.AreEqual("aid", ((ABAssociation)result[0]).AReference.id);
            }
        }
    }
}
