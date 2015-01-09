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
    public class StatTableValueGenerator
    {
        public class SampleClass
        {
            public string Name { get; set; }
            public string Ethnicity { get; set; }
            public string Gender { get; set; }
        }

        [TestMethod, Obsolete]
        public void TestWithGoodPropertyName()
        {
            var input = new SampleClass
            {
                Ethnicity = "OldEthnicityType.AsianOrPacificIslander",
                Gender = "M"
            };

            var generator = new StatTableWorkItem
            {
                DataProvider = new MemoryStatDataProvider(),
                PropertyToSet = "Name",
                PropertiesToLook = new[] { "Ethnicity", "Gender" }
            };

            generator.DoWork(input, null);
            Assert.AreEqual("test", input.Name);
            Console.WriteLine(input.Name);
        }

        [TestMethod, Obsolete]
        [ExpectedException(typeof(InvalidPropertyException))]
        public void TestWithBadPropertyName()
        {
            var input = new SampleClass();

            var generator = new StatTableWorkItem
            {
                DataProvider = new MemoryStatDataProvider(),
                PropertyToSet = "UnknownPropertyName",
                PropertiesToLook = new string[] { }
            };

            generator.DoWork(input, null);
        }
    }
}
