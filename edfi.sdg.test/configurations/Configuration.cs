using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace edfi.sdg.test.configurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using edfi.sdg.configurations;

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

            var knownTypes = new List<Type>();

            var properties =
                typeof(sdg.configurations.Configuration).GetProperties()
                    .Where(p => p.GetCustomAttribute<GenericXmlElementAttribute>() != null);

            foreach (var propertyInfo in properties)
            {
                var attrib = propertyInfo.GetCustomAttribute<GenericXmlElementAttribute>();
                knownTypes.AddRange(attrib.GetKnownTypes(propertyInfo));
            }

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(sdg.configurations.Configuration), knownTypes.Distinct().ToArray());
            serializer.Serialize(Console.Out, configuration);
        }
    }
}
