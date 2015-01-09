namespace edfi.sdg.generators
{
    using System.Linq;

    public class PropertyPopulatorWorkItem : WorkItem
    {
        public string[] Classes { get; set; }

        public override object[] DoWork(object input, interfaces.IConfiguration configuration)
        {
            if (Classes.Contains(input.GetType().Name))
            {
                new Generator().Populate(input);
            }
            return new object[] { input };
        }
    }
}
