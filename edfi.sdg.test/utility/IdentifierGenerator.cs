using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace edfi.sdg.test.utility
{
    [TestClass]
    public class IdentifierGenerator
    {
        [TestMethod]
        public void RoundtripGuidValue()
        {
            var expected = Guid.NewGuid();
            var identifier = EdFi.SampleDataGenerator.Utility.IdentifierGenerator.Create(expected);
            var actual = EdFi.SampleDataGenerator.Utility.IdentifierGenerator.ToGuid(identifier);
            Assert.AreEqual(expected, actual);
        }
    }
}
