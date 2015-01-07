using System;
using System.Linq;
using System.Xml.Serialization;
using edfi.sdg.interfaces;
using edfi.sdg.utility;

namespace edfi.sdg.generators
{
    /// <summary>
    /// Create a number of objects of type T and put them on the work queue
    /// </summary>
    [Serializable]
    public class StatTableValueGenerator : Generator
    {
        public StatDataProviderBase DataProvider { get; set; }

        public string[] PropertiesToLook { get; set; }

        [XmlAttribute]
        public string PropertyToSet { get; set; }

        public override object[] Generate(object input, IConfiguration configuration)
        {
            var results = new[] {input};

            var statAttributeList = PropertiesToLook.Select(property => (string) input.GetValue(property)).ToArray();

            var result = DataProvider.GetNextValue(statAttributeList);
            
            input.SetValue(PropertyToSet, result);

            return results;
        }
    }
}
