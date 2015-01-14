using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdFi.SampleDataGenerator.Test.Generators
{
    using System.Linq;

    [TestClass]
    public class PropertyExtractor
    {
        // ReSharper disable once ClassNeverInstantiated.Local
        private class ClassA
        {
            public string StringProperty { get; set; }
            public int IntProperty { get; set; }
        }

        private class ClassB
        {
            public ClassA ClassAProperty { get; set; }
            public string[] StringArrayProperty { get; set; }
        }

        private class ClassC : ClassB
        {
            public double DoubleProperty { get; set; }
        }

        [TestMethod]
        public void RetrievesAllPropertyPaths()
        {
            var result = EdFi.SampleDataGenerator.Generators.PropertyExtractor.ExtractPropertyMetadata(typeof(ClassC));
            var propertyPaths = result.Select(x => x.PropertyPath).ToArray();
            Assert.IsTrue(propertyPaths.Contains("ClassAProperty"));
            Assert.IsTrue(propertyPaths.Contains("StringArrayProperty"));
            Assert.IsTrue(propertyPaths.Contains("ClassAProperty.StringProperty"));
            Assert.IsTrue(propertyPaths.Contains("ClassAProperty.IntProperty"));
            Assert.IsTrue(propertyPaths.Contains("DoubleProperty"));
        }

        [TestMethod]
        public void RetrievesAllPropertyNames()
        {
            var result = EdFi.SampleDataGenerator.Generators.PropertyExtractor.ExtractPropertyMetadata(typeof(ClassC));
            var propertyNames = result.Select(x => x.PropertyInfo.Name).ToArray();
            Assert.IsTrue(propertyNames.Contains("ClassAProperty"));
            Assert.IsTrue(propertyNames.Contains("StringArrayProperty"));
            Assert.IsTrue(propertyNames.Contains("StringProperty"));
            Assert.IsTrue(propertyNames.Contains("IntProperty"));
            Assert.IsTrue(propertyNames.Contains("DoubleProperty"));
        }

        [TestMethod]
        public void RetrievesAllPropertyTypes()
        {
            var result = EdFi.SampleDataGenerator.Generators.PropertyExtractor.ExtractPropertyMetadata(typeof(ClassC));
            var propertyTypes = result.Select(x => x.PropertyInfo.PropertyType).ToArray();
            Assert.IsTrue(propertyTypes.Contains(typeof(ClassA)));
            Assert.IsTrue(propertyTypes.Contains(typeof(string[])));
            Assert.IsTrue(propertyTypes.Contains(typeof(string)));
            Assert.IsTrue(propertyTypes.Contains(typeof(int)));
            Assert.IsTrue(propertyTypes.Contains(typeof(double)));
        }
    }
}
