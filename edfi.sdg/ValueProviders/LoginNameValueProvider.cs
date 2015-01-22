using System;
using System.Linq;
using EdFi.SampleDataGenerator.Utility;

namespace EdFi.SampleDataGenerator.ValueProviders
{
    public class LoginNameValueProvider : ValueProvider
    {
        public override object GetValue(object[] dependsOn)
        {
            if (dependsOn.IsNullOrEmpty())
                return string.Format("{0}@{1}", DateTime.Now.Ticks);

            var elements = dependsOn.Select(d => d.ToString());

            return string.Join(".", elements).ToLower();
        }
    }
}