using System;
using System.Xml.Serialization;

namespace EdFi.SampleDataGenerator.Distributions
{
    [Serializable]
    [XmlInclude(typeof(ConstantDistribution))]
    [XmlInclude(typeof(RangeDistribution))]
    [XmlInclude(typeof(BucketedDistribution))]
    public abstract class DistributionBase
    {
        protected static readonly Random Rand = new Random();
        public abstract T Next<T>();
        public abstract T[] Shuffled<T>();
    }
}
