using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace edfi.sdg.test.configurations
{
    using System;
    using System.Xml.Serialization;

    [TestClass]
    public class Configuration
    {
        /// <summary>
        /// This test should always pass. 
        /// Its purpose is to serialize Configuration.Default which then may be used for customization.
        /// </summary>
        [TestMethod]
        public void GenerateDefaultConfiguration()
        {
            var configuration = sdg.configurations.Configuration.DefaultConfiguration;
            var serializer = new XmlSerializer(typeof(sdg.configurations.Configuration));
            serializer.Serialize(Console.Out, configuration);
        }
    }
}
