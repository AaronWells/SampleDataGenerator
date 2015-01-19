using System;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using EdFi.SampleDataGenerator.Configurations;

namespace EdFi.SampleDataGenerator.WorkItems
{
    [Serializable]
    public abstract class WorkItem
    {
        /// <summary>
        /// classes is a regular expression of type names that this generator should work on.
        /// for example:
        ///     ^EdFi\.SampleDataGenerator\.Models\.((Student)|(Parent))$
        /// matches:
        ///     EdFi.SampleDataGenerator.Models.Student
        ///     EdFi.SampleDataGenerator.Models.Parent
        /// but not:
        ///     EdFi.SampleDataGenerator.Models.Students
        ///     EdFi.SampleDataGenerator.Models4Student
        ///     EdFi.SampleDataGenerator.Models.Parent.Other
        /// </summary>
        [XmlAttribute]
        public string ClassFilterRegex { get; set; }

        [XmlIgnore]
        public int Id { get; set; }

        protected WorkItem()
        {
            ClassFilterRegex = "^.*$"; // default is to match all classes
        }

        public object[] DoWork(object input, IConfiguration configuration)
        {
            if (input == null || Regex.IsMatch(input.GetType().FullName, ClassFilterRegex))
            {
                return DoWorkImplementation(input, configuration);
            }
            return new[] { input };
        }

        protected abstract object[] DoWorkImplementation(object input, IConfiguration configuration);
    }
}
