using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace edfi.sdg.test.configurations
{
    using System;

    [TestClass]
    public class Configuration
    {
        /// <summary>
        /// This test verifies that we can serialize Configuration.Default. 
        /// The output may also be used as a basis for customization.
        /// </summary>
        [TestMethod]
        public void GenerateDefaultConfiguration()
        {
            var configuration = sdg.configurations.Configuration.DefaultConfiguration;
            var serializer = sdg.configurations.Configuration.ConfigurationSerializer();
            serializer.Serialize(Console.Out, configuration);
        }
    }
}
