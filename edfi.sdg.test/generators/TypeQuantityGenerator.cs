using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace edfi.sdg.test.generators
{
    using edfi.sdg.configurations;
    using edfi.sdg.generators;
    using edfi.sdg.interfaces;
    using edfi.sdg.test.classes;

    [TestClass]
    public class TypeQuantityGenerator
    {
        /// <summary>
        /// The TypeQuantityGenerator should distribute work among multiple queued work items, 
        /// and create exactly the specified quantity of items.
        /// </summary>
        [TestMethod]
        public void DistributeWork()
        {
            const int SpecifiedQuantity = 1000000;

            var generatedQuantity = 0;

            var queue = new TestQueue();

            var configuration = new Configuration
                                    {
                                        MaxQueueWrites = 50
                                    };

            var generator = (IGenerator)new edfi.sdg.generators.TypeQuantityGenerator<SerializableTestClass>()
                                {
                                    Id = 1,
                                    QuantitySpecifier = new ConstantQuantity { Quantity = SpecifiedQuantity },
                                };

            generator.Generate(null, queue, configuration);

            while (!queue.IsEmpty)
            {
                var task = queue.ReadObjectAsync();
                task.Wait();
                var obj = task.Result as IGenerator;
                if (obj == null)
                {
                    generatedQuantity++; //count one item
                }
                else
                {
                    obj.Generate(null, queue, configuration);
                }
            }
            Assert.AreEqual(SpecifiedQuantity, generatedQuantity);
        }
    }
}
