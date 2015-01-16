using System.Collections.Generic;
using EdFi.SampleDataGenerator.Generators;
using EdFi.SampleDataGenerator.ValueProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdFi.SampleDataGenerator.Test.ValueProviders
{
    public class CopyPropertyValueProviderTests
    {
        [TestClass]
        public class SingleObjectCopyTests
        {
            class SomeClass
            {
                public string StringProp1 { get; set; }
                public string StringProp2 { get; set; }
            }

            [TestMethod]
            public void SimpleTest()
            {
                var ruleSet = new List<ValueRule>
                {
                    new ValueRule
                    {
                        Class = "SomeClass",
                        PropertySpecifier = "StringProp2",
                        ValueProvider = new CopyPropertyValueProvider {LookupProperties = new[] {"StringProp1"}}
                    }
                };

                var generator = new Generator(ruleSet);

                var instance = new SomeClass { StringProp1 = "SomeValue" };

                generator.Populate(instance);

                Assert.AreEqual("SomeValue", instance.StringProp2);
            }
        }
    }
}
