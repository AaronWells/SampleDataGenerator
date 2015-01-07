using edfi.sdg.generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace edfi.sdg.test.generators
{
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
        public void TestGetNextValue()
        {
            var input = new SampleClass
            {
                Ethnicity = "OldEthnicityType.AsianOrPacificIslander",
                Gender = "M"
            };

            var generator = new StatTableValueGenerator
            {
                StatTableName = "FamilyName",
                PropertyToSet = "Name",
                PropertiesToLook = new[] { "Ethnicity", "Gender" }
            };

            generator.Generate(input, null);
            Assert.AreNotEqual(null, input.Name);
            Console.WriteLine(input.Name);
        }
    }
}
