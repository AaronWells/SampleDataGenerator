namespace edfi.sdg.test.classes
{
    using System;
    using System.Linq;

    [Serializable()]
    public class SerializableTestClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TestEnum TestEnum { get; set; }

        public SerializableTestClass()
        {
            var guid = Guid.NewGuid();
            this.Id = guid.ToByteArray().First();
            this.Name = guid.ToString("N");
        }
    }
}