using System;
using System.Collections.Generic;
using EdFi.SampleDataGenerator.Generators;
using EdFi.SampleDataGenerator.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdFi.SampleDataGenerator.Test.Utility
{
    [TestClass]
    public class HelperTests
    {
        [TestMethod]
        public void TestTracer()
        {
            var x = new object();
            var list = new LinkedList<TraceObject>(
                new List<TraceObject>
                {
                    new TraceObject{ObjectToTrace = x, PropertyTypeName = "T1", PropertyName = "NO-NAME" },
                    new TraceObject{ObjectToTrace = x, PropertyTypeName = "T2", PropertyName = "P2" },
                    new TraceObject{ObjectToTrace = x, PropertyTypeName = "T3", PropertyName = "P3" },
                });

            var possibilities = Helper.RuleMatchPossibilities(list.Last, "xxx");

            foreach (var possibility in possibilities)
            {
                Console.WriteLine(possibility);
            }

            Assert.AreEqual("T3::xxx", possibilities[0]);
            Assert.AreEqual("T2::P3.xxx", possibilities[1]);
            Assert.AreEqual("T1::P2.P3.xxx", possibilities[2]);
        }
    }
}
