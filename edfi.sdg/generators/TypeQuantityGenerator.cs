namespace edfi.sdg.generators
{
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
        where T : IComplexObjectType, new()
    {
        /// <summary>
        /// Number of objects to create
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ConstantQuantity", typeof(ConstantQuantity))]
        [System.Xml.Serialization.XmlElementAttribute("NormalQuantity", typeof(NormalQuantity))]
        [System.Xml.Serialization.XmlElementAttribute("ChiQuantity", typeof(ChiQuantity))]
        [System.Xml.Serialization.XmlElementAttribute("ChiSquareQuantity", typeof(ChiSquareQuantity))]
        [System.Xml.Serialization.XmlElementAttribute("BucketedQuantity", typeof(BucketedQuantity))]
        public Quantity QuantitySpecifier { get; set; }

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
            var qty = QuantitySpecifier.Next();
            if (qty > configuration.MaxQueueWrites)
            {
                var gen1 = new TypeQuantityGenerator<T> { QuantitySpecifier = new ConstantQuantity { Quantity = qty / 2 } };
                queueWriter.WriteObject(gen1);

                var gen2 = new TypeQuantityGenerator<T> { QuantitySpecifier = new ConstantQuantity { Quantity = qty / 2 + qty % 2 } };
                queueWriter.WriteObject(gen2);
            }
            else
            {
                Parallel.For(0, qty,
                    number =>
                    {
                        var model = new T();

                        var complexObject = model as ComplexObjectType;
                        if (complexObject != null)
                        {
                            complexObject.id = IdentifierGenerator.Create();
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
