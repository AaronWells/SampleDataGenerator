using System;
using System.Xml.Serialization;

namespace EdFi.SampleDataGenerator.ValueProvider
{
    [Serializable]
    [XmlInclude(typeof(DatabaseStatDataValueProvider))]
    public abstract class StatDataValueProviderBase
    {
        public abstract string GetNextValue(string[] lookupProperties);
    }
}
