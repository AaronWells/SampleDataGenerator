using System;
using System.Collections.Generic;
using EdFi.SampleDataGenerator.Generators;
using EdFi.SampleDataGenerator.Models;
using EdFi.SampleDataGenerator.Repository;
using EdFi.SampleDataGenerator.Utility;
using EdFi.SampleDataGenerator.ValueProvider;
using EdFi.SampleDataGenerator.WorkItems;
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

    public class SampleType
    {
        public int IntProperty { get; set; }
        public string StringProperty1 { get; set; }
        public string StringProperty2 { get; set; }
    }

    public class SampleType2 : ComplexObjectType
    {
        public string id { get; set; }
    }

    [TestClass]
    public class StatTableValueGenerator
    {
        public class SampleClass
        {
            public string Name { get; set; }
            public string Ethnicity { get; set; }
            public string Gender { get; set; }
        }

        [TestMethod, Obsolete]
        public void TestWithGoodPropertyName()
        {
            var input = new SampleClass
            {
                Ethnicity = "Ethnicity.AsianOrPacificIslander",
                Gender = "M"
            };

            var generator = new StatTableWorkItem
            {
                DataRepository = new MemoryStatDataRepository(),
                PropertyToSet = "Name",
                PropertiesToLook = new[] { "Ethnicity", "Gender" },
            };

            generator.DoWork(input, null);
            Assert.AreEqual("test", input.Name);
            Console.WriteLine(input.Name);
        }

        [TestMethod, Obsolete]
        [ExpectedException(typeof(InvalidPropertyException))]
        public void TestWithBadPropertyName()
        {
            var input = new SampleClass();

            var generator = new StatTableWorkItem
            {
                DataRepository = new MemoryStatDataRepository(),
                PropertyToSet = "SampleClass.UnknownPropertyName",
                PropertiesToLook = new string[] { }
            };

            generator.DoWork(input, null);
        }

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

        [TestMethod]
        public void TestWithParameters()
        {
            var rulePack = new List<ValueRule>
            {
                new ValueRule
                {
                    Class = "*",
                    PropertySpecifier = "StringProperty2",
                    ValueProvider = new StatTableValueProvider
                    {
                        DataRepository = new MemoryStatDataRepository(),
                        LookupProperties = new[] {"StringProperty1"}
                    }
                }
            };
            var generator = new Generator(rulePack);

            var instance = new SampleType {StringProperty1 = "Prop1"};

            generator.Populate(instance);

            Assert.AreEqual("Prop1", instance.StringProperty1);
            Assert.AreEqual("Prop1", instance.StringProperty2);
        }
    }

    [TestClass]
    public class TestComposition
    {
        private class Composite
        {
            public string Value { get; set; }
        }

        private class BaseClass
        {
            public string Value { get; set; }
            public Composite CompositeProperty1 { get; set; }
            public Composite CompositeProperty2 { get; set; }
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
        public void TestWithRulesWithParameters()
        {
            var rulePack = new List<ValueRule>
            {
                new ValueRule
                {
                    Class = "*",
                    PropertySpecifier = "Value",
                    ValueProvider = new SampleValueProvider {MyValue = "TestValue", LookupProperties = new []{"param1"}}
                }
            };

            var generator = new Generator(rulePack);

            var instance = new BaseClass();

            generator.Populate(instance);

            Assert.AreEqual("TestValue", instance.Value);
            Assert.AreEqual("TestValue", instance.CompositeProperty1.Value);
        }
    }
}