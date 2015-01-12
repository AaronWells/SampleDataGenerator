using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdFi.SampleDataGenerator.Test.Utility
{
    [TestClass]
    public class IdentifierGenerator
    {
        [TestMethod]
        public void RoundTripGuidValue()
        {
            var expected = Guid.NewGuid();
            var identifier = SampleDataGenerator.Utility.IdentifierGenerator.Create(expected);
            var actual = SampleDataGenerator.Utility.IdentifierGenerator.ToGuid(identifier);
            Assert.AreEqual(expected, actual);
        }
    }
}
