using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Generators.CustomGenerators;
using EdFi.SampleDataGenerator.Generators.StandardGenerators;
using EdFi.SampleDataGenerator.Models;
using EdFi.SampleDataGenerator.Utility;
using EdFi.SampleDataGenerator.ValueProvider;

namespace EdFi.SampleDataGenerator.Generators
{
    public class Generator
    {
        private readonly IEnumerable<ValueRule> _rulePack;

        public Generator(IEnumerable<ValueRule> rulePack)
        {
            _rulePack = rulePack;
        }
        
        public void Populate(object input)
        {
            var type = input.GetType();
            // can't populate non-class types
            if (!type.IsClass)
            {
                throw new ArgumentException(string.Format("cannot populate non-class type: '{0}'", type.FullName));
            }

            var properties = type.GetProperties();

            if (input is ComplexObjectType)
            {
                properties = properties.Where(p => p.Name != "id").ToArray();
            }

            foreach (var property in properties)
            {
                var value =
                    new RuleBaseGenerator(_rulePack).Handle(property) ??
                    new StandardGenerator().Handle(property);

                input.SetPropertyValue(property.Name, value);
            }
        }
    }


}
