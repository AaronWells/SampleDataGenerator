using System;
using edfi.sdg.entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace edfi.sdg.test.database
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestGetNextValue()
        {
            var value = new DataAccess().GetNextValue("FamilyName", new []{"OldEthnicityType.AsianOrPacificIslander", "OldEthnicityType.Hispanic"});

            Console.WriteLine(value);
            Assert.AreNotEqual(null, value);
        }
    }
}
