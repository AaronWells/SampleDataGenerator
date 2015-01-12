using System;
using EdFi.SampleDataGenerator.Data;

namespace EdFi.SampleDataGenerator.ValueProvider
{
    [Serializable]
    public class DatabaseStatDataValueProvider : StatDataValueProviderBase
    {
        private readonly DataAccess _dataAccess;

        public DatabaseStatDataValueProvider()
        {
            _dataAccess = new DataAccess();
        }

        public string StatTableName { get; set; }

        public override string GetNextValue(string[] lookupProperties)
        {
            return _dataAccess.GetNextValue(StatTableName, lookupProperties);
        }
    }

}
