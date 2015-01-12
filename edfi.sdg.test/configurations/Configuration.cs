using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdFi.SampleDataGenerator.Test.Configurations
{
    [TestClass]
    public class Configuration
    {
        /// <summary>
        /// This test verifies that we can serialize Configuration. Default. 
        /// The output may also be used as a basis for customization.
        /// </summary>
        [TestMethod]
        public void GenerateDefaultConfiguration()
        {
            var configuration = SampleDataGenerator.Configurations.Configuration.DefaultConfiguration;
            var serializer = SampleDataGenerator.Configurations.Configuration.ConfigurationSerializer();
            serializer.Serialize(Console.Out, configuration);
        }
    }
}
