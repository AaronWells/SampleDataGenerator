namespace edfi.sdg.configurations
{
    using System.Collections.Generic;

    using edfi.sdg.generators;
    using edfi.sdg.interfaces;
    using edfi.sdg.models;

    [System.SerializableAttribute]
    public partial class Configuration : IConfiguration
    {
        public int MaxQueueWrites { get; set; }

        public int NumThreads { get; set; }

        public string WorkQueueName { get; set; }

        /// <summary>
        /// string is the matching criteria (*.Name.FirstName)
        /// </summary>
        public ValueRule[] ValueRules { get; set; }

        private WorkItem[] workItems;

        /// <summary>
        /// List of generators to run. Each generator type must exist in the attributes listed here. 
        /// Generics MUST also list every valid value for their parameters for XML serialization to work.
        /// </summary>
        [GenericXmlElementAttribute(BaseTargetType = typeof(ComplexObjectType))]
        public WorkItem[] WorkItems {
            get
            {
                return this.workItems;
            }
            set
            {
                var idx = 1;
                this.workItems = value;
                foreach (var generator in this.workItems)
                {
                    generator.Id = idx++;
                }
            }
        }
    }
}
