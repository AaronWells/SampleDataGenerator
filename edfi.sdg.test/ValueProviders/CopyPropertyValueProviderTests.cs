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

        [TestClass]
        public class CompositeObjectCopyTests
        {
            class SomeClass
            {
                public string StringProp { get; set; }
                public InnerClass ClassProp { get; set; }
            }

            class InnerClass
            {
                public string StringProp { get; set; }
            }

            [TestMethod]
            public void TopDownTest()
            {
                var ruleSet = new List<ValueRule>
                {
                    new ValueRule
                    {
                        Class = "SomeClass",
                        PropertySpecifier = "StringProp",
                        ValueProvider =
                            new CopyPropertyValueProvider {LookupProperties = new[] {"ClassProp.StringProp"}}
                    },
                    new ValueRule
                    {
                        Class = "InnerClass",
                        PropertySpecifier = "StringProp",
                        ValueProvider = new SampleValueProvider {MyValue = "TheValue"}
                    }
                };

                var generator = new Generator(ruleSet);

                var instance = new SomeClass();

                generator.Populate(instance);

                Assert.AreEqual("TheValue", instance.StringProp);
            }

            [TestMethod]
            public void BottomUpTest()
            {
                var ruleSet = new List<ValueRule>
                {
                    new ValueRule
                    {
                        Class = "SomeClass",
                        PropertySpecifier = "StringProp",
                        ValueProvider = new SampleValueProvider {MyValue = "TheValue"}
                    },
                    new ValueRule
                    {
                        Class = "InnerClass",
                        PropertySpecifier = "StringProp",
                        ValueProvider = new CopyPropertyValueProvider {LookupProperties = new[] {"{parent}.StringProp"}}
                    }
                };

                var generator = new Generator(ruleSet);

                var instance = new SomeClass();

                generator.Populate(instance);

                Assert.AreEqual("TheValue", instance.StringProp);
                Assert.AreEqual("TheValue", instance.StringProp);
            }
        }

        [TestClass]
        public class ComplextObjectCopyTest
        {
            public class SomeClass
            {
                public InnerClass ClassProp1 { get; set; }
                public InnerClass ClassProp2 { get; set; }
            }

            public class InnerClass
            {
                public string StringProp { get; set; }
            }

            [TestMethod]
            public void EntireClassTest()
            {
                var ruleSet = new List<ValueRule>
                {
                    new ValueRule
                    {
                        Class = "SomeClass",
                        PropertySpecifier = "ClassProp2",
                        ValueProvider = new CopyPropertyValueProvider {LookupProperties = new []{"ClassProp1"}}
                    },
                    new ValueRule
                    {
                        Class = "SomeClass",
                        PropertySpecifier = "ClassProp1.StringProp",
                        ValueProvider = new SampleValueProvider {MyValue = "TheValue"}
                    }
                };

                var generator = new Generator(ruleSet);

                var instance = new SomeClass();

                generator.Populate(instance);

                Assert.AreEqual("TheValue", instance.ClassProp1.StringProp);
                Assert.AreEqual("TheValue", instance.ClassProp2.StringProp);
                Assert.IsFalse(instance.ClassProp1.Equals(instance.ClassProp2));
            }
        }
    }
}
