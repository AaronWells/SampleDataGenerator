using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using edfi.sdg.entity;
using edfi.sdg.interfaces;

namespace edfi.sdg.generators
{
    /// <summary>
    /// Create a number of objects of type T and put them on the work queue
    /// </summary>
    [Serializable]
    public class StatTableValueGenerator : Generator
    {
        private readonly DataAccess _dataAccess;

        public StatTableValueGenerator()
        {
            _dataAccess = new DataAccess();
        }

        public string StatTableName { get; set; }

        public string[] PropertiesToLook { get; set; }

        [XmlAttribute]
        public string PropertyToSet { get; set; }

        public override object[] Generate(object input, IConfiguration configuration)
        {
            var results = new[] {input};

            // property to set:
            var type = input.GetType();
            var propertyToSet = type.GetProperty(PropertyToSet);
            
            if (propertyToSet == null) return results; //todo: log error/warning

            var statAttributeList = new List<string>();
            
            foreach (var item in PropertiesToLook)
            {
                var property = type.GetProperty(item);
                var propetyValue = property.GetValue(input);
                statAttributeList.Add((string) propetyValue);
            }

            var result = _dataAccess.GetNextValue(StatTableName, statAttributeList);
            propertyToSet.SetValue(input, result);

            return results;
        }
    }
}
