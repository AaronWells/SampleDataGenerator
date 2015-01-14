using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdFi.SampleDataGenerator.Test.Generators
{
    using System.Linq;
    using System.Runtime.CompilerServices;

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
            var propertyPaths = result.SelectMany(x => x.PropertyPaths.Select(y => y.ToString())).ToArray();
            Assert.IsTrue(propertyPaths.Contains("ClassC::DoubleProperty"));
            Assert.IsTrue(propertyPaths.Contains("ClassC::ClassAProperty"));
            Assert.IsTrue(propertyPaths.Contains("ClassA::StringProperty"));
            Assert.IsTrue(propertyPaths.Contains("ClassC::ClassAProperty.StringProperty"));
            Assert.IsTrue(propertyPaths.Contains("ClassA::IntProperty"));
            Assert.IsTrue(propertyPaths.Contains("ClassC::ClassAProperty.IntProperty"));
            Assert.IsTrue(propertyPaths.Contains("ClassC::StringArrayProperty"));
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

        [TestMethod]
        public void ParentPropertiesAreSet()
        {
            var result = EdFi.SampleDataGenerator.Generators.PropertyExtractor.ExtractPropertyMetadata(typeof(ClassC));
            var parentTypes = result.Where(y => y.Type == typeof(ClassA)).Select(x => x.ParentPropertyMetadata.Type).Distinct().ToArray();
            Assert.IsTrue(parentTypes.Contains(typeof(ClassC)));
            Assert.IsFalse(parentTypes.Contains(typeof(ClassB)));
            Assert.IsFalse(parentTypes.Contains(typeof(ClassA)));
        }

        [TestMethod]
        public void FindDependentAttribute()
        {
            var metadatas = EdFi.SampleDataGenerator.Generators.PropertyExtractor.ExtractPropertyMetadata(typeof(ClassC));
            var propMetadata = metadatas.First(x => x.CompareTo("ClassA::StringProperty") == 0);
            var absolutePath = propMetadata.ResolveRelativePath("{parent}.DoubleProperty");
        }

    }
}
