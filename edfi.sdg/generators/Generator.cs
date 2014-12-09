namespace edfi.sdg.generators
{
    using System;

    using edfi.sdg.interfaces;

    public abstract class Generator: IGenerator
    {
        protected static readonly Random Rnd = new Random();

        public int Id { get; set; }

        public string Name { get; set; }

        public abstract void Generate(object input, IQueueWriter queueWriter, IConfiguration configuration);
    }
}
