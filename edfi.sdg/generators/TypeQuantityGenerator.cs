namespace edfi.sdg.generators
{
    using System.Threading.Tasks;

    using edfi.sdg.interfaces;
    using edfi.sdg.messaging;

    [System.SerializableAttribute()]
    public class TypeQuantityGenerator<T> : Generator
        where T : new()
    {
        public long Quantity { get; set; }
        
        /// <summary>
        /// Create a number of objects and place them on the queue, 
        /// or if the task is too big, split the task in two and put those tasks back on the queue
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

                var gen2 = new TypeQuantityGenerator<T> { Id = Id, Name = Name, Quantity = Quantity / 2+ Quantity % 2 };
                queueWriter.WriteObject(gen2);
            }
            else
            {
                Parallel.For(0, Quantity,
                    number =>
                        {
                            var envelope = new WorkEnvelope
                                               {
                                                   NextStep = this.Id,
                                                   Model = new T()
                                               };
                            queueWriter.WriteObject(envelope);
                        });
            }
        }
    }
}
