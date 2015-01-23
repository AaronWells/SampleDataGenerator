using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdFi.SampleDataGenerator.Test.Writers
{
    using System;
    using System.IO;

    using EdFi.SampleDataGenerator.Models;
    using EdFi.SampleDataGenerator.Writers;

    [TestClass]
    public class InterchangeWriterTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var interchangeTypes = new Type[] { typeof(InterchangeStudentParent) };

            InterchangeWriter.WriteInterchanges(Directory.GetCurrentDirectory(), interchangeTypes);
        }
    }
}
