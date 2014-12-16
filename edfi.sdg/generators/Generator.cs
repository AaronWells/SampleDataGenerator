namespace edfi.sdg.generators
{
    using System;
    using System.Xml.Serialization;

    using edfi.sdg.interfaces;

    [Serializable]
    public abstract class Generator : IGenerator
    {
        protected static readonly Random Rnd = new Random();

        [XmlIgnore]
        public int Id { get; set; }

        public abstract object[] Generate(object input, IConfiguration configuration);
    }
}
