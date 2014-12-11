namespace edfi.sdg.generators
{
    using System.Linq;
    using System.Threading.Tasks;

    using edfi.sdg.interfaces;
    using edfi.sdg.messaging;
    using edfi.sdg.models;
    using edfi.sdg.utility;

    /// <summary>
    /// Create a number of objects of type T and put them on the work queue
    /// </summary>
    /// <typeparam name="T">The type of object to generate</typeparam>
    [System.SerializableAttribute()]
    public class TypeQuantityGenerator<T> : Generator
        where T : new()
    {
        /// <summary>
        /// Number of objects to create
        /// </summary>
        public long Quantity { get; set; }

        /// <summary>
        /// Create a number of objects and place them on the queue, 
        /// or if there are too many, split the task in two and put those tasks back on the queue.
        /// Initialize the Id property.
        /// </summary>
        /// <param name="input">Ignored</param>
        /// <param name="queueWriter">Writes one or more elements to queue</param>
        /// <param name="configuration">uses MaxQueueWrites property</param>
        public override void Generate(object input, IQueueWriter queueWriter, IConfiguration configuration)
        {
            if (Quantity > configuration.MaxQueueWrites)
            {
                var gen1 = new TypeQuantityGenerator<T> { Id = Id, Name = Name, Quantity = Quantity / 2 };
                queueWriter.WriteObject(gen1);

                var gen2 = new TypeQuantityGenerator<T> { Id = Id, Name = Name, Quantity = Quantity / 2 + Quantity % 2 };
                queueWriter.WriteObject(gen2);
            }
            else
            {
                Parallel.For(0, Quantity,
                    number =>
                    {
                        var model = new T();

                        var complexObject = model as ComplexObjectType;
                        if (complexObject != null)
                        {
                            complexObject.id = IdentifierGenerator.CreateNew();
                        }

                        var envelope = new WorkEnvelope
                                           {
                                               NextStep = this.Id,
                                               Model = model
                                           };
                        queueWriter.WriteObject(envelope);
                    });
            }
        }
    }
}
