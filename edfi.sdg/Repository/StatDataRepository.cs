using System;
using System.Xml.Serialization;

namespace EdFi.SampleDataGenerator.Repository
{
    [Serializable]
    [XmlInclude(typeof(DatabaseStatDataRepository))]
    public abstract class StatDataRepository
    {
        public abstract string GetNextValue(string[] lookupProperties);
    }
}