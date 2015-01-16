using EdFi.SampleDataGenerator.Models;
using EdFi.SampleDataGenerator.ValueProviders;
using EdFi.SampleDataGenerator.WorkItems;

namespace EdFi.SampleDataGenerator.Configurations
{
    [System.SerializableAttribute]
    public partial class Configuration : IConfiguration
    {
        public int MaxQueueWrites { get; set; }

        public int ThreadCount { get; set; }

        public string WorkQueueName { get; set; }
        
        public ValueRule[] GetValueRules()
        {
            return ValueRules;
        }

        /// <summary>
        /// string is the matching criteria (*.Name.FirstName)
        /// </summary>
        public ValueRule[] ValueRules { get; set; }

        private WorkItem[] _workFlow;

        /// <summary>
        /// List of generators to run. Each generator type must exist in the attributes listed here. 
        /// Generics MUST also list every valid value for their parameters for XML serialization to work.
        /// </summary>
        [GenericXmlElement(BaseTargetType = typeof(ComplexObjectType))]
        public WorkItem[] WorkFlow {
            get
            {
                return _workFlow;
            }
            set
            {
                var idx = 1;
                _workFlow = value;
                foreach (var generator in _workFlow)
                {
                    generator.Id = idx++;
                }
            }
        }
    }
}
