using edfi.sdg.generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace edfi.sdg.test.generators
{
    [TestClass]
    public class when_try_to_get_the_next_stat_value_from_a_well_populated_stat_table
    {
        private string Result { get; set; }

        [TestInitialize]
        public void InitTest()
        {
            var generator = new StatTableValueGenerator
            {
                StatTableName = "FamilyName",
                Attributes = new[] { "OldEthnicityType.AsianOrPacificIslander", "OldEthnicityType.Hispanic" }
            };

            Result = (string) generator.Generate(null, null)[0];
        }

        [TestMethod]
        public void result_should_be_null()
        {
            Assert.AreNotEqual(null, Result);
        }

        [TestMethod]
        public void should_return_a_value()
        {
            Console.WriteLine(Result);
        }
    }
}
