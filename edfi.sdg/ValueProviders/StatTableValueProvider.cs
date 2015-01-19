using System;
using System.Linq;
using EdFi.SampleDataGenerator.Repository;

namespace EdFi.SampleDataGenerator.ValueProviders
{
    [Serializable]
    public class StatTableValueProvider : ValueProvider
    {
        public StatDataRepository DataRepository { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="dependsOn">This might be any number of dependent properties that stored procedure uses. 
        /// We expect all the objects in this array to be string. </param>
        public override object GetValue(object[] dependsOn)
        {
            var dependentStringList = dependsOn.Select(GetStringValue).ToArray();
            return DataRepository.GetNextValue(dependentStringList);
        }

        private static string GetStringValue(object obj)
        {
            if (obj.GetType().IsEnum)
            {
                // get the fully qualified name:
                return string.Format("{0}.{1}", obj.GetType().Name, obj);
            }
            return obj.ToString();
        }
    }
}
