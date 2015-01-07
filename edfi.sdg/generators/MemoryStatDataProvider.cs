using System;
using edfi.sdg.entity;
using edfi.sdg.interfaces;

namespace edfi.sdg.generators
{
    [Serializable]
    public class DatabaseStatDataProvider : StatDataProviderBase
    {
        private readonly DataAccess _dataAccess;

        public DatabaseStatDataProvider()
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
