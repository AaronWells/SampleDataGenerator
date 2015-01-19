using System;
using System.Diagnostics;
using EdFi.SampleDataGenerator.Distributions;
using EdFi.SampleDataGenerator.Test.Classes;
using EdFi.SampleDataGenerator.ValueProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdFi.SampleDataGenerator.Test.Generators
{
    [TestClass]
    public class DistributedEnumValueProviderTests
    {
        [TestMethod]
        public void TestDistribution()
        {
            var count = 0.0;
            for (var i = 0; i < 10000; i++)
            {
                var valueProvider = new DistributedEnumValueProvider<TestEnum>
                {
                    Distribution = new BucketedDistribution
                    {
                        Weightings = new[]
                        {
                            new Weighting {Value = TestEnum.Alpha, Weight = 0.5},
                            new Weighting {Value = TestEnum.Charlie, Weight = 0.5},
                        }
                    }
                };

                var value = (TestEnum)valueProvider.GetValue();

                switch (value)
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
