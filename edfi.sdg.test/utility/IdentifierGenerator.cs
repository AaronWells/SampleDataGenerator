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
            var identifier = sdg.utility.IdentifierGenerator.CreateFromGuid(expected);
            var actual = sdg.utility.IdentifierGenerator.ToGuid(identifier);
            Assert.AreEqual(expected, actual);
        }
    }
}
