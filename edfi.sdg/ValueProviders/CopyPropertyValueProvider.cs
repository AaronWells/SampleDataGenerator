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
        /// returns the value of the provided relative property. It uses the Dependencies which should carry 
        /// one and only one relative property
        /// </summary>
        public override object GetValue(params string[] dependsOn)
        {
            if(LookupProperties.Length != 1)
                throw new ArgumentException("cannot pass multiple parameters to the CopyPropertyValueProvider.GetValue() method");

            var dependentPath = LookupProperties.First();

            Console.WriteLine(dependentPath);

            throw new NotImplementedException();
        }
    }
}