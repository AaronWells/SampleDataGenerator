using System;
using System.Xml.Serialization;
using edfi.sdg.generators;

namespace edfi.sdg.interfaces
{
    [Serializable]
    [XmlInclude(typeof(DatabaseStatDataProvider))]
    public abstract class StatDataProviderBase
    {
        public abstract string GetNextValue(string[] lookupProperties);
    }
}
