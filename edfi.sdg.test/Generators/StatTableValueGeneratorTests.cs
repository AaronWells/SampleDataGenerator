using System;
using System.Collections.Generic;
using EdFi.SampleDataGenerator.Generators;
using EdFi.SampleDataGenerator.Models;
using EdFi.SampleDataGenerator.Repository;
using EdFi.SampleDataGenerator.ValueProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdFi.SampleDataGenerator.Test.Generators
{
    [Serializable]
    public class MemoryStatDataRepository : StatDataRepository
    {
        public override string GetNextValue(string[] lookupProperties)
        {
            return "test";
        }
    }

    public class SampleType2 : ComplexObjectType
    {
        public string id { get; set; }
    }

    [TestClass]
    public class StatTableValueGeneratorTests
    {
        /// <summary>
        /// When to be populated object is a <see cref="ComplexObjectType"/>, 'id' should not be poplated
        /// </summary>
        [TestMethod]
        public void TestId()
        {
            var rulePack = new List<ValueRule>
            {
                new ValueRule
                {
                    PropertySpecifier = "StringProperty2",
                    ValueProvider = new StatTableValueProvider
                    {
                        DataRepository = new MemoryStatDataRepository(),
                        LookupProperties = new string[] {}
                    }
                }
            };
            var generator = new Generator(rulePack);

            var instance = new SampleType2 {id = "SpecificId"};

            generator.Populate(instance);

            Assert.AreEqual("SpecificId", instance.id);
        }
    }

    [TestClass]
    public class TestComposition
    {
        public class BaseClass
        {
            public string Value { get; set; }
            public Composite CompositeProperty1 { get; set; }
            public Composite CompositeProperty2 { get; set; }
        }

        public class Composite
        {
            public string Value { get; set; }
        }

        public class CompositDataProvider : ValueProvider
        {
            public override object GetValue(object[] dependsOn)
            {
                return new Composite {Value = "inner value"};
            }
        }

        [TestMethod]
        public void TestWithRootRelatedRules()
        {
            var rulePack = new List<ValueRule>
            {
                new ValueRule
                {
                    Class = "BaseClass",
                    PropertySpecifier = "Value",
                    ValueProvider = new SampleValueProvider {MyValue = "TestValue"}
                }
            };

            var generator = new Generator(rulePack);

            var instance = new BaseClass();

            generator.Populate(instance);

            Assert.AreEqual("TestValue", instance.Value);
            Assert.AreEqual(null, instance.CompositeProperty1.Value);
            Assert.AreEqual(null, instance.CompositeProperty2.Value);
        }

        [TestMethod]
        public void TestWithInnerClassRuleRules()
        {
            var rulePack = new List<ValueRule>
            {
                new ValueRule
                {
                    Class = "Composite",
                    PropertySpecifier = "Value",
                    ValueProvider = new SampleValueProvider {MyValue = "TestValue"}
                }
            };

            var generator = new Generator(rulePack);

            var instance = new BaseClass();

            generator.Populate(instance);

            Assert.AreEqual(null, instance.Value);
            Assert.AreEqual("TestValue", instance.CompositeProperty1.Value);
            Assert.AreEqual("TestValue", instance.CompositeProperty2.Value);
        }

        [TestMethod]
        public void TestWithRulesAndTighterCriteria()
        {
            var rulePack = new List<ValueRule>
            {
                new ValueRule
                {
                    Class = "BaseClass",
                    PropertySpecifier = "CompositeProperty1.Value",
                    ValueProvider = new SampleValueProvider {MyValue = "TestValue"}
                }
            };

            var generator = new Generator(rulePack);

            var instance = new BaseClass();

            generator.Populate(instance);

            Assert.AreEqual(null, instance.Value);
            Assert.AreEqual("TestValue", instance.CompositeProperty1.Value);
            Assert.AreEqual(null, instance.CompositeProperty2.Value);
        }

        [TestMethod]
        public void TestWithRulesWithDependencies1()
        {
            var rulePack = new List<ValueRule>
            {
                new ValueRule
                {
                    Class = "BaseClass",
                    PropertySpecifier = "Value",
                    ValueProvider = new SampleValueProvider {MyValue = "TestValue", LookupProperties = new []{"CompositeProperty2.Value"}}
                },
            };

            var generator = new Generator(rulePack);

            var instance = new BaseClass();

            generator.Populate(instance);

            Assert.AreEqual("TestValue", instance.Value);
            Assert.AreEqual(null, instance.CompositeProperty1.Value);
        }

        [TestMethod]
        public void TestWithRulesWithDependencies2()
        {
            var rulePack = new List<ValueRule>
            {
                new ValueRule
                {
                    Class = "BaseClass",
                    PropertySpecifier = "CompositeProperty1.Value",
                    ValueProvider = new SampleValueProvider {MyValue = "TestValue", LookupProperties = new []{"{parent}.CompositeProperty2.Value"}}
                }
            };

            var generator = new Generator(rulePack);

            var instance = new BaseClass();

            generator.Populate(instance);

            Assert.AreEqual(null, instance.Value);
            Assert.AreEqual("TestValue", instance.CompositeProperty1.Value);
        }

        [TestMethod]
        public void TestWithRulesWithDependencies3()
        {
            var rulePack = new List<ValueRule>
            {
                new ValueRule
                {
                    Class = "BaseClass",
                    PropertySpecifier = "CompositeProperty2.Value",
                    ValueProvider = new SampleValueProvider {MyValue = "TestValue", LookupProperties = new []{"{parent}.CompositeProperty1.Value"}}
                }, 
                new ValueRule
                {
                    Class = "BaseClass",
                    PropertySpecifier = "CompositeProperty1",
                    ValueProvider = new CompositDataProvider { LookupProperties = new []{"CompositeProperty2"} }
                },
            };

            var generator = new Generator(rulePack);

            var instance = new BaseClass();

            generator.Populate(instance);

            Assert.AreEqual(null, instance.Value);
            Assert.AreEqual("inner value", instance.CompositeProperty1.Value);
        }
    }
}