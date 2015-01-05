using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace edfi.sdg.entity.test
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void next_random_family_name_should_not_be_null()
        {
            var value = new DataAccess().GetNextValue("FamilyName", new []{"OldEthnicityType.AsianOrPacificIslander", "OldEthnicityType.Hispanic"});

            Console.WriteLine(value);
            Assert.AreNotEqual(null, value);
        }
    }
}
