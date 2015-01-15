using System;
using System.Linq;
using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Configurations;
using EdFi.SampleDataGenerator.Repository;
using EdFi.SampleDataGenerator.Utility;
using EdFi.SampleDataGenerator.WorkItems;

namespace EdFi.SampleDataGenerator.ValueProvider
{
    /// <summary>
    /// Create a number of objects of type T and put them on the work queue
    /// </summary>
    [Serializable]
    [Obsolete]
    public class StatTableWorkItem : WorkItem
    {
        public StatDataRepository DataRepository { get; set; }

        public string[] PropertiesToLook { get; set; }

        [XmlAttribute]
        public string PropertyToSet { get; set; }

        protected override object[] DoWorkImplementation(object input, IConfiguration configuration)
        {
            var results = new[] { input };

            var statAttributeList = PropertiesToLook.Select(property => (string)input.GetPropertyValue(property)).ToArray();

            var result = DataRepository.GetNextValue(statAttributeList);

            input.SetPropertyValue(PropertyToSet, result);

            return results;
        }
    }
}