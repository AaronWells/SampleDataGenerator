using System;
using EdFi.SampleDataGenerator.Models;

namespace EdFi.SampleDataGenerator.Test.Classes
{
    [Serializable]
    public class SerializableTestClass: IComplexObjectType
    {
        public string id { get; set; }
        public TestEnum TestEnum { get; set; }

        public SerializableTestClass()
        {
            var guid = Guid.NewGuid();
            id = guid.ToString("N");
        }
    }
}