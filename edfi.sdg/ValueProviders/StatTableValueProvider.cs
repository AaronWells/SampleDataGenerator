using System;
using EdFi.SampleDataGenerator.Repository;

namespace EdFi.SampleDataGenerator.ValueProviders
{
    [Serializable]
    public class StatTableValueProvider : ValueProvider
    {
        public StatDataRepository DataRepository { get; set; }

        public override object GetValue(params string[] dependsOn)
        {
            return DataRepository.GetNextValue(dependsOn);
        }
    }
}
