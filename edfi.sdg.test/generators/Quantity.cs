using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace edfi.sdg.test.generators
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;

    [TestClass]
    public class Quantity
    {
        [TestMethod]
        public void SerializationTests()
        {
            var allPassed = true;
            var assembly = Assembly.GetAssembly(typeof(AssemblyLocator));
            foreach (var type in assembly.GetTypes().Where(t => t.Namespace == "edfi.sdg.generators" && !t.IsAbstract && !t.IsGenericTypeDefinition).OrderBy(t => t.Name))
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
