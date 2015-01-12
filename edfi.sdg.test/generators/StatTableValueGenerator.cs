using System;
using EdFi.SampleDataGenerator.Utility;
using EdFi.SampleDataGenerator.ValueProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdFi.SampleDataGenerator.Test.Generators
{
    [Serializable]
    public class MemoryStatDataValueProvider : StatDataValueProviderBase
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
                Ethnicity = "Ethnicity.AsianOrPacificIslander",
                Gender = "M"
            };

            var generator = new StatTableWorkItem
            {
                DataValueProvider = new MemoryStatDataValueProvider(),
                PropertyToSet = "Name",
                PropertiesToLook = new[] { "Ethnicity", "Gender" },
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
                DataValueProvider = new MemoryStatDataValueProvider(),
                PropertyToSet = "SampleClass.UnknownPropertyName",
                PropertiesToLook = new string[] { }
            };

            generator.DoWork(input, null);
        }
    }
}
