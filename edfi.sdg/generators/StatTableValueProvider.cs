using System;
using System.Linq;
using System.Xml.Serialization;
using edfi.sdg.interfaces;
using edfi.sdg.utility;

namespace edfi.sdg.generators
{
    [Serializable]
    public class StatTableValueProvider : ValueProvider
    {
        public StatDataProviderBase DataProvider { get; set; }

        public override object GetValue()
        {
            var empty = new string[] { };
            return DataProvider.GetNextValue(empty);
        }

        public override object GetValue(params string[] lookupPropertyValues)
        {
            return DataProvider.GetNextValue(lookupPropertyValues);
        }
    }


    /// <summary>
    /// Create a number of objects of type T and put them on the work queue
    /// </summary>
    [Serializable]
    [Obsolete]
    public class StatTableWorkItem : WorkItem
    {
        public StatDataProviderBase DataProvider { get; set; }

        public string[] PropertiesToLook { get; set; }

        [XmlAttribute]
        public string PropertyToSet { get; set; }

        protected override object[] DoWorkImplementation(object input, IConfiguration configuration)
        {
            var results = new[] { input };

            var statAttributeList = PropertiesToLook.Select(property => (string)input.GetValue(property)).ToArray();

            var result = DataProvider.GetNextValue(statAttributeList);

            input.SetValue(PropertyToSet, result);

            return results;
        }
    }
}
