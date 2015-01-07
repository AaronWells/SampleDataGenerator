using edfi.sdg.generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace edfi.sdg.test.generators
{
    [TestClass]
    public class StatTableValueGeneratorTest
    {
        [TestMethod]
        public void result_should_be_null()
        {
            var generator = new StatTableValueGenerator
            {
                StatTableName = "FamilyName",
                Attributes = new[] { "OldEthnicityType.AsianOrPacificIslander", "OldEthnicityType.Hispanic" }
            };

            var result = (string) generator.Generate(null, null)[0];
            Assert.AreNotEqual(null, result);
            Console.WriteLine(result);
        }
    }
}
