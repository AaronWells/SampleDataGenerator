namespace edfi.sdg.generators
{
    using System;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    using edfi.sdg.interfaces;
    [Serializable]
    public abstract class WorkItem : IWorkItem
    {
        /// <summary>
        /// classes is a regular expression of type names that this generator should work on.
        /// for example:
        ///     ^edfi\.sdg\.models\.((Student)|(Parent))$
        /// matches:
        ///     edfi.sdg.models.Student
        ///     edfi.sdg.models.Parent
        /// but not:
        ///     edfi.sdg.models.Students
        ///     edfi.sdg.models4Student
        ///     edfi.sdg.models.Parent.Other
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
