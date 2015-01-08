namespace edfi.sdg.generators
{
    using System;
    using System.Xml.Serialization;

    using edfi.sdg.interfaces;
    [Serializable]
    public abstract class WorkItem : IWorkItem
    {
        protected static readonly Random Rnd = new Random();

        [XmlIgnore]
        public int Id { get; set; }

        public abstract object[] DoWork(object input, IConfiguration configuration);
    }
}
