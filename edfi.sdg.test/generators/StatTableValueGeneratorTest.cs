using edfi.sdg.generators;
using edfi.sdg.interfaces;
using edfi.sdg.utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace edfi.sdg.test.generators
{
    [Serializable]
    public class MemoryStatDataProvider : StatDataProviderBase
    {
        public override string GetNextValue(string[] lookupProperties)
        {
            return "test";
        }
    }

    [TestClass]
    public class StatTableValueGeneratorTest
    {
        public class SampleClass
        {
            public string Name { get; set; }
            public string Ethnicity { get; set; }
            public string Gender { get; set; }
        }

        [TestMethod]
        public void TestWithGoodPropertyName()
        {
            var input = new SampleClass
            {
                Ethnicity = "OldEthnicityType.AsianOrPacificIslander",
                Gender = "M"
            };

            var generator = new StatTableValueGenerator
            {
                DataProvider = new MemoryStatDataProvider(),
                PropertyToSet = "SampleClass.Name",
                PropertiesToLook = new[] { "SampleClass.Ethnicity", "SampleClass.Gender" }
            };

            generator.Generate(input, null);
            Assert.AreEqual("test", input.Name);
            Console.WriteLine(input.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPropertyException))]
        public void TestWithBadPropertyName()
        {
            var input = new SampleClass();

            var generator = new StatTableValueGenerator
            {
                DataProvider = new MemoryStatDataProvider(),
                PropertyToSet = "SampleClass.UnknownPropertyName",
                PropertiesToLook = new string[] { }
            };

            generator.Generate(input, null);
        }
    }
}
