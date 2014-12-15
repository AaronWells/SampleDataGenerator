namespace edfi.sdg.configurations
{
    using edfi.sdg.generators;
    using edfi.sdg.interfaces;
    using edfi.sdg.models;

    [System.SerializableAttribute]
    public partial class Configuration : IConfiguration
    {
        public int MaxQueueWrites { get; set; }

        public int NumThreads { get; set; }

        public string WorkQueueName { get; set; }

        private Generator[] generators;

        /// <summary>
        /// List of generators to run. Each generator type must exist in the attributes listed here. 
        /// Generics MUST also list every valid value for their parameters for XML serialization to work.
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("StudentGenerator", typeof(TypeQuantityGenerator<Student>))]
        [System.Xml.Serialization.XmlElementAttribute("SexGenerator", typeof(DistributedEnumValueGenerator<SexType>))]
        [System.Xml.Serialization.XmlElementAttribute("OldEthnicityGenerator", typeof(DistributedEnumValueGenerator<OldEthnicityType>))]
        public Generator[] Generators {
            get
            {
                return this.generators;
            }
            set
            {
                var idx = 1;
                this.generators = value;
                foreach (var generator in this.generators)
                {
                    generator.Id = idx++;
                }
            }
        }
    }
}
