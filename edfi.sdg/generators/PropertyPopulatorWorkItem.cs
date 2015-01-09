namespace edfi.sdg.generators
{
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    public class PropertyPopulatorWorkItem : WorkItem
    {
        protected override object[] DoWorkImplementation(object input, interfaces.IConfiguration configuration)
        {
            Generator.Populate(input);
            return new object[] { input };
        }
    }
}
