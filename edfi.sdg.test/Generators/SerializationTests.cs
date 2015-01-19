using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdFi.SampleDataGenerator.Test.Generators
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void TestAllClasses()
        {
            var allPassed = true;
            var assembly = Assembly.Load(new AssemblyName("EdFi.SampleDataGenerator"));
            var typesToBeSerialized = assembly.GetTypes()
                .Where(t => t.Namespace.In("EdFi.SampleDataGenerator.WorkItems", "EdFi.SampleDataGenerator.ValueProvider"))
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Where(t => !t.Name.StartsWith("<>")) // to exclude anonymous types
                .OrderBy(t => t.Name);

            foreach (var type in typesToBeSerialized)
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
