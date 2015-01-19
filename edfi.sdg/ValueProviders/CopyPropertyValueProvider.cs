using System;
using System.Linq;

namespace EdFi.SampleDataGenerator.ValueProviders
{
    /// <summary>
    /// Provides a value based on the relative path of the dependent parameter. 
    /// </summary>
    public class CopyPropertyValueProvider : ValueProvider
    {
        /// <summary>
        /// returns the value of the provided relative property. It uses the LookupProperties to find
        /// the property from one and only one relative property in the <paramref name="dependsOn"/> object hierarchy
        /// </summary>
        public override object GetValue(params object[] dependsOn)
        {
            if (dependsOn.Length != 1)
                throw new ArgumentException("cannot pass multiple parameters to the CopyPropertyValueProvider.GetValue() method");

            var theObject = dependsOn[0];

            return theObject;
        }
    }
}