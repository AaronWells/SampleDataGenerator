using System;
using System.Collections.Generic;
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

        public IEnumerable<string> Attributes { get; set; }

        public override object[] Generate(object input, IConfiguration configuration)
        {
            var results = new object[1];

            results[0] = _dataAccess.GetNextValue(StatTableName, Attributes);

            return results;
        }
    }
}
