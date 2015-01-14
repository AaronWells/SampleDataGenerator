using System;
using EdFi.SampleDataGenerator.Data;

namespace EdFi.SampleDataGenerator.Repository
{
    [Serializable]
    public class DatabaseStatDataRepository : StatDataRepositoryBase
    {
        private readonly DataAccess _dataAccess;

        public DatabaseStatDataRepository()
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
