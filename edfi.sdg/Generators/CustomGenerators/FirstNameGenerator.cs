using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EdFi.SampleDataGenerator.Configurations;
using EdFi.SampleDataGenerator.Generators.StandardGenerators;

namespace EdFi.SampleDataGenerator.Generators.CustomGenerators
{
    public class RuleBaseGenerator : ICustomGenerator
    {
        private readonly IEnumerable<ValueRule> _rulePack;
        private ValueRule _rule;

        public RuleBaseGenerator(IEnumerable<ValueRule> rulePack)
        {
            _rulePack = rulePack;
        }

        public bool CanHandle(PropertyInfo property)
        {
            _rule = _rulePack.SingleOrDefault(r => r.Criteria == property.Name);
            return _rule != null;
        }

        public object Handle(PropertyInfo property)
        {
            return CanHandle(property)
                ? _rule.ValueProvider.GetValue()
                : new NullValueGenerator().Handle(property);
        }
    }
}