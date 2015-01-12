namespace edfi.sdg.test.models
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Models
    {
        /// <summary>
        /// Verify that xml serialization and deserialization works for all the generated model classes
        /// </summary>
        [TestMethod]
        public void SerializationTests()
        {
            var allPassed = true;
            var assembly = Assembly.Load(new AssemblyName("EdFi.SampleDataGenerator"));
            foreach (var type in assembly.GetTypes().Where(t => t.Namespace == "EdFi.SampleDataGenerator.Models" && !t.IsAbstract).OrderBy(t => t.Name))
                using (var stream = new MemoryStream())
                {
                    try
                    {
                        var serializer = new XmlSerializer(type);
                        serializer.Serialize(stream, Activator.CreateInstance(type));
                        stream.Seek(0, SeekOrigin.Begin);
                        serializer.Deserialize(stream);
                        Console.WriteLine("passed: " + type);
                    }
                    catch
                    {
                        allPassed = false;
                        Console.WriteLine("FAILED: " + type);
                    }
                }
            Assert.IsTrue(allPassed);
        }
    }
}
