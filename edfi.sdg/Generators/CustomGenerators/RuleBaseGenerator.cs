using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using EdFi.SampleDataGenerator.Generators.StandardGenerators;
using EdFi.SampleDataGenerator.Utility;
using EdFi.SampleDataGenerator.ValueProviders;

namespace EdFi.SampleDataGenerator.Generators.CustomGenerators
{
    public class RuleBaseGenerator : ICustomGenerator
    {
        private readonly LinkedListNode<TraceObject> _parentObject;
        private readonly IEnumerable<ValueRule> _rulePack;
        private ValueRule _rule;

        public RuleBaseGenerator(LinkedListNode<TraceObject> parentObject, IEnumerable<ValueRule> rulePack)
        {
            _parentObject = parentObject;
            _rulePack = rulePack;
        }

        public bool CanHandle(PropertyInfo property)
        {
            var possibilites = Helper.RuleMatchPossibilities(_parentObject, property.Name);
            var matchingRules = _rulePack.Where(r => MatchesCriteria(r.Path, possibilites)).ToList();
            if (matchingRules.Count() > 1)
            {
                throw new ConfigurationErrorsException(string.Format(
                    "Rule with criteria '{0}' has a conflict with {1}", matchingRules.First().Path,
                    matchingRules.Skip(1).First().Path));
            }
            _rule = matchingRules.FirstOrDefault();
            return _rule != null;
        }

        public object Handle(PropertyInfo property)
        {
            return CanHandle(property)
                ? _rule.ValueProvider.GetValue()
                : new NullValueGenerator().Handle(property);
        }

        private static bool MatchesCriteria(string criteria, ICollection<string> possibilites)
        {
            return possibilites != null && possibilites.Contains(criteria);
        }
    }
}