using System;
using EdFi.SampleDataGenerator.Data;

namespace EdFi.SampleDataGenerator.Repository
{
    [Serializable]
    public class DatabaseStatDataRepository : StatDataRepository
    {
        private readonly DataRepository _dataRepository;

        public DatabaseStatDataRepository()
        {
            _dataRepository = new DataRepository();
        }

        public string StatTableName { get; set; }

        public override string GetNextValue(string[] lookupProperties)
        {
            return _dataRepository.GetNextValue(StatTableName, lookupProperties);
        }
    }
}
