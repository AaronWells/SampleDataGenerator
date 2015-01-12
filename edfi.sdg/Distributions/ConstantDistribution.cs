using System;

namespace EdFi.SampleDataGenerator.Distributions
{
    [Serializable]
    public class ConstantDistribution : DistributionBase
    {
        public object Value { get; set; }

        public override T Next<T>()
        {
            return (T)Value;
        }

        public override T[] Shuffled<T>()
        {
            return new[] { (T)Value };
        }
    }
}