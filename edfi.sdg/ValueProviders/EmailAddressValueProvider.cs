using System;
using System.Linq;
using EdFi.SampleDataGenerator.Utility;

namespace EdFi.SampleDataGenerator.ValueProviders
{
    public class EmailAddressValueProvider : ValueProvider
    {
        private const string DefaultDomain = "not-a-valid-domain.com";

        public string Domain { get; set; }

        public EmailAddressValueProvider()
        {
            Domain = DefaultDomain;
        }

        public override object GetValue(object[] dependsOn)
        {
            if (dependsOn.IsNullOrEmpty())
                return string.Format("{0}@{1}", DateTime.Now.Ticks, Domain);

            var elements = dependsOn.Select(d => d.ToString());

            return string.Format("{0}@{1}", string.Join(".", elements), Domain);
        }
    }
}