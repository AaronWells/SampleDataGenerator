using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Utility;
using EdFi.SampleDataGenerator.ValueProviders;
using log4net;

namespace EdFi.SampleDataGenerator.Generators
{
    public class Generator
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Generator));

        private readonly IEnumerable<ValueRule> _rulePack;

        public Generator(IEnumerable<ValueRule> rulePack)
        {
            _rulePack = rulePack;
        }

        public void Populate(object input)
        {
            var propertyExtract = CreatePropertyExtract(input);
            SetPropertyRuleMap(propertyExtract);
            var graph = PrepareDependencyGraph(propertyExtract);

            foreach (var propertyMetadata in graph.GetEvaluationOrder())
            {
                var containingObject = input.LocateObject(propertyMetadata.AbsolutePath.PropertyChain);
                
                // first find any matching rule:
                if (propertyMetadata.BestMatchingRule != null)
                {
                    // use the rule to populate property
                    var valueProvider = propertyMetadata.BestMatchingRule.ValueProvider;
                    var dependentProperties = ResolveDependencyCollection(propertyMetadata, valueProvider.LookupProperties, propertyExtract);
                    var @params = new List<object>();
                    foreach (var dependentProperty in dependentProperties)
                    {
                        var containingDependentObject = input.LocateObject(dependentProperty.AbsolutePath.PropertyChain);
                        var dependentObjectValue = containingDependentObject.GetPropertyValue(dependentProperty.PropertyInfo.Name);
                        @params.Add(dependentObjectValue);
                    }
                    var value = valueProvider.GetValue(@params.ToArray());
                    if (value.GetType().IsCompositeType())
                    {
                        value = value.Clone();
                    }

                    // todo: if value-provider returns a serialized xml, it should be deserialized at this step
                    // value = Deserialize(value)

                    containingObject.SetPropertyValue(propertyMetadata.PropertyInfo.Name, value);
                }
                else if (propertyMetadata.PropertyInfo.PropertyType.IsCompositeType())
                {
                    // instantiate the property with a new class
                    var value = Activator.CreateInstance(propertyMetadata.PropertyInfo.PropertyType);
                    containingObject.SetPropertyValue(propertyMetadata.PropertyInfo.Name, value);
                }
                // else : leave it alone
            }
        }

        private static PropertyMetadata ResolveDependency(PropertyMetadata baseProperty, string propertyToResolve, IEnumerable<PropertyMetadata> propertyHierarchy)
        {
            return propertyHierarchy.Single(p => p.Matches(baseProperty.ResolveRelativePath(propertyToResolve)));
        }

        private static IEnumerable<PropertyMetadata> ResolveDependencyCollection(PropertyMetadata baseProperty, IEnumerable<string> propertiesToResolve, IEnumerable<PropertyMetadata> propertyHierarchy)
        {
            return propertiesToResolve.Select(d => ResolveDependency(baseProperty, d, propertyHierarchy));
        }

        private void SetPropertyRuleMap(IEnumerable<PropertyMetadata> propertyExtract)
        {
            foreach (var propertyMetadata in propertyExtract)
            {
                propertyMetadata.BestMatchingRule = GetBestMatchingRule(propertyMetadata);
            }
        }

        private static List<PropertyMetadata> CreatePropertyExtract(object input)
        {
            return PropertyExtractor.ExtractPropertyMetadata(input.GetType()).ToList();
        }

        private DirectedGraph<PropertyMetadata> PrepareDependencyGraph(List<PropertyMetadata> propertyExtract)
        {
            var graph = new DirectedGraph<PropertyMetadata>(propertyExtract);

            foreach (var propertyMetadata in propertyExtract)
            {
                var dependencies = new List<PropertyMetadata>
                {
                    propertyMetadata.ParentPropertyMetadata
                };
                
                var bestMatchingRule = propertyMetadata.BestMatchingRule;

                if (bestMatchingRule != null && bestMatchingRule.HasDependency)
                {
                    // add dependencies
                    var list = ResolveDependencyCollection(propertyMetadata, bestMatchingRule.ValueProvider.LookupProperties,
                        propertyExtract);
                    dependencies.AddRange(list);
                }

                graph.SetDependencies(propertyMetadata, dependencies);
            }
            return graph;
        }

        private ValueRule GetBestMatchingRule(PropertyMetadata propertyMetadata)
        {
            ValueRule bestMatchingRule = null;
            foreach (var rule in _rulePack)
            {
                if (propertyMetadata.Matches(rule.Path) &&
                    (bestMatchingRule == null || rule.Path.Length < bestMatchingRule.Path.Length))
                {
                    bestMatchingRule = rule;
                }
            }

            if (bestMatchingRule != null)
                _logger.Debug(string.Format("best matching rule for '{0}': {1}", propertyMetadata.PropertyInfo.Name, bestMatchingRule));

            return bestMatchingRule;
        }
    }
}
