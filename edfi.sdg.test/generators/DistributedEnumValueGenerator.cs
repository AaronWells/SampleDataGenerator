using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace edfi.sdg.test.generators
{
    using System.Diagnostics;

    using edfi.sdg.configurations;
    using edfi.sdg.generators;
    using edfi.sdg.test.classes;

    [TestClass]
    public class DistributedEnumValueGenerator
    {
        [TestMethod]
        public void TestRandomDistribution()
        {

            var queue = new TestQueue();
            var config = Configuration.DefaultConfiguration;

            for (var i = 0; i < 10000; i++)
            {
                var generator = new DistributedEnumValueGenerator<TestEnum>
                {
                    Distribution = new BucketedDistribution()
                    {
                        Weightings = new Weighting[]
                        {
                            new Weighting{Value = TestEnum.Alpha, Weight = 0.5},
                            new Weighting{Value = TestEnum.Charlie, Weight = 0.5},
                        }
                    },
                    Property = "SerializableTestClass.TestEnum",
                };
                var obj = new SerializableTestClass();
                foreach (var tmp in generator.Generate(obj, config))
                {
                    queue.WriteObject(tmp);
                }
            }
            var count = 0.0;
            while (!queue.IsEmpty)
            {
                var task = queue.ReadObjectAsync();
                task.Wait();
                var obj2 = (SerializableTestClass)task.Result;
                switch (obj2.TestEnum)
                {
                    case TestEnum.Alpha:
                        count += 1.0;
                        break;
                    case TestEnum.Bravo:
                        break;
                    case TestEnum.Charlie:
                        break;
                    case TestEnum.Delta:
                        break;
                    case TestEnum.Foxtrot:
                        break;
                    case TestEnum.Golf:
                        break;
                    case TestEnum.Hotel:
                        break;
                    case TestEnum.Igloo:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
            var result = count / 10000.0;
            Debug.WriteLine("{0} percent were alpha", result);
            Assert.IsTrue(Math.Abs(result - 0.5) < 0.05);
        }
    }
}
