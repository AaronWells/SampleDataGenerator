using System.Reflection;
using edfi.sdg.generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace edfi.sdg.test.toBeCombined
{
    public class SampleType1
    {
        public int IntProperty { get; set; }
    }

    public class SampleType2
    {
        public string StringProperty { get; set; }
    }

    public class CompositeType
    {
        public SampleType1 Property1 { get; set; }
    }

    [TestClass]
    public class GeneratorTests
    {
        private Assembly _assembly;

        [TestInitialize]
        public void Init()
        {
            _assembly = Assembly.GetAssembly(typeof(GeneratorTests)); // get this assembly
        }

        [TestMethod]
        public void Test1()
        {
            var generator = new Generator(_assembly);

            var instance = generator.GetMeA("edfi.sdg.test.toBeCombined.SampleType1") as SampleType1;
            
            Assert.AreEqual(0, instance.IntProperty);
        }

        [TestMethod]
        public void Test2()
        {
            var generator = new Generator(_assembly);

            var instance = generator.GetMeA("edfi.sdg.test.toBeCombined.SampleType2") as SampleType2;
            
            Assert.AreEqual(null, instance.StringProperty);
        }

        [TestMethod]
        public void TestCompositeType()
        {
            var generator = new Generator(_assembly);

            var instance = generator.GetMeA("edfi.sdg.test.toBeCombined.CompositeType") as CompositeType;
            
            Assert.AreNotEqual(null, instance);
            Assert.AreNotEqual(null, instance.Property1);
        }

    }
}
