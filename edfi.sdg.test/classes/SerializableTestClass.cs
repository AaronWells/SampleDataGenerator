using EdFi.SampleDataGenerator.Models;

namespace edfi.sdg.test.classes
{
    using System;

    [Serializable()]
    public class SerializableTestClass: IComplexObjectType
    {
        public string id { get; set; }
        public TestEnum TestEnum { get; set; }

        public SerializableTestClass()
        {
            var guid = Guid.NewGuid();
            this.id = guid.ToString("N");
        }
    }
}