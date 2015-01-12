using System;
using EdFi.SampleDataGenerator.Configurations;
using EdFi.SampleDataGenerator.Models;
using EdFi.SampleDataGenerator.Quantity;
using EdFi.SampleDataGenerator.Utility;

namespace EdFi.SampleDataGenerator.WorkItems
{
    /// <summary>
    /// Create a number of objects of type T and put them on the work queue
    /// </summary>
    /// <typeparam name="T">The type of object to generate</typeparam>
    [Serializable]
    public class TypeQuantityWorkItem<T> : WorkItem where T : IComplexObjectType, new()
    {
        /// <summary>
        /// Number of objects to create
        /// </summary>
        public QuantityBase QuantitySpecifier { get; set; }

        /// <summary>
        /// Create a number of objects and place them on the queue, 
        /// or if there are too many, split the task in two and put those tasks back on the queue.
        /// Initialize the Id property.
        /// </summary>
        /// <param name="input">ignored</param>
        /// <param name="configuration">uses MaxQueueWrites property</param>
        /// <returns>an array of work items</returns>
        protected override object[] DoWorkImplementation(object input, IConfiguration configuration)
        {
            object[] results;
            var qty = QuantitySpecifier.Next();
            if (qty > configuration.MaxQueueWrites)
            {
                results = new object[]
                {
                    new TypeQuantityWorkItem<T> {Id = Id, QuantitySpecifier = new ConstantQuantity {Quantity = qty/2}},
                    new TypeQuantityWorkItem<T> {Id = Id, QuantitySpecifier = new ConstantQuantity {Quantity = qty/2 + qty%2}}
                };
            }
            else
            {
                results = new object[qty];
                for (var i = 0; i < qty; i++)
                {
                    results[i] = new T { id = IdentifierGenerator.Create() };
                }
            }
            return results;
        }
    }
}
