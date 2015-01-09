using System.Collections.Generic;
using System.Reflection;
using edfi.sdg.generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace edfi.sdg.test.generators
{
    [TestClass]
    public class GeneratorTests
    {
        private const string ThisClassNamespace = "edfi.sdg.test.generators";
        private static readonly Assembly _assembly = Assembly.GetAssembly(typeof(GeneratorTests)); // get this assembly


        [TestClass]
        public class SimpleTypeTests
        {
            public class SampleType
            {
                public int IntProperty { get; set; }
                public string StringProperty { get; set; }
            }

            [TestMethod]
            public void TestStringProperty()
            {
                var generator = new Generator(_assembly);

                var instance = generator.GetMeA(ThisClassNamespace, "GeneratorTests+SimpleTypeTests+SampleType") as SampleType;

                Assert.AreEqual(null, instance.StringProperty);
            }

            [TestMethod]
            public void TestIntProperty()
            {
                var generator = new Generator(_assembly);

                var instance = generator.GetMeA(ThisClassNamespace, "GeneratorTests+SimpleTypeTests+SampleType") as SampleType;

                Assert.AreEqual(0, instance.IntProperty);
            }

        }

        [TestClass]
        public class EnumTypeTests
        {
            public enum SampleEnum { Value1, Value2 }

            public class EnumType
            {
                public SampleEnum EnumProperty { get; set; }
            }

            [TestMethod]
            public void TestEnumProperty()
            {
                var generator = new Generator(_assembly);

                var instance = generator.GetMeA(ThisClassNamespace, "GeneratorTests+EnumTypeTests+EnumType") as EnumType;

                Assert.AreNotEqual(null, instance);
                Assert.AreEqual(SampleEnum.Value1, instance.EnumProperty);
            }
        }

        [TestClass]
        public class CompositeTypeTests
        {
            public class ChildType
            {
                public int IntProperty { get; set; }
            }

            public class ParentType
            {
                public ChildType ChileProperty { get; set; }
            }

            [TestMethod]
            public void TestCompositeType()
            {

                var generator = new Generator(_assembly);

                var instance = generator.GetMeA(ThisClassNamespace, "GeneratorTests+CompositeTypeTests+ParentType") as ParentType;
            
                Assert.AreNotEqual(null, instance);
                Assert.AreNotEqual(0, instance.ChileProperty);
            }
        }

        [TestClass]
        public class SimpleTestWithRules
        {
            public class SampleType
            {
                public string StringProperty1 { get; set; }
                public string StringProperty2 { get; set; }
            }

            [TestMethod]
            public void TestStringProperty()
            {
                const string trigger = ThisClassNamespace + "." + "GeneratorTests+SimpleTestWithRules+SampleType.StringProperty2";

                var generator = new Generator(_assembly)
                {
                    RuleSets = new List<RuleSet>
                    {
                        new RuleSet {Action = TestAction, RuleName = "RuleName", RuleTrigger = trigger }
                    }
                };

                var instance = generator.GetMeA(ThisClassNamespace, "GeneratorTests+SimpleTestWithRules+SampleType") as SampleType;

                Assert.AreEqual("SomeValue", instance.StringProperty2);
            }

            public object TestAction()
            {
                return "SomeValue";
            }
        }

    }
}
