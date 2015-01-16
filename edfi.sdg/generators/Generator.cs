using System;
using System.Collections.Generic;
using System.Linq;
using EdFi.SampleDataGenerator.Utility;
using EdFi.SampleDataGenerator.ValueProviders;

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
            var propertyExtract = CreatePropertyExtract(input);
            SetPropertyRuleMap(propertyExtract);
            var graph = PrepareDependencyGraph(propertyExtract);

            foreach (var propertyMetadata in graph.GetEvaluationOrder())
            {
                var containingObject = LocateObject(input, propertyMetadata.AbsolutePath.PathSegment.ToList());
                
                // first find any matching rule:
                if (propertyMetadata.BestMatchingRule != null)
                {
                    // use the rule to populate property
                    var value = propertyMetadata.BestMatchingRule.ValueProvider.GetValue();

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

        private object LocateObject(object input, List<string> segments)
        {
            if (!segments.Any()) return null; // this is error condition

            if (segments.Count() == 1)
                return input;

            var firstSegment = input.GetPropertyValue(segments.First());
            return LocateObject(firstSegment, segments.Skip(1).ToList());
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
                    dependencies.AddRange(
                        bestMatchingRule.ValueProvider.Dependencies
                            .Select(d => propertyExtract.Single(p => p.Matches(propertyMetadata.ResolveRelativePath(d)))));
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
            return bestMatchingRule;
        }
    }

    public class TraceObject
    {
        public object ObjectToTrace { get; set; }
        public string PropertyTypeName { get; set; }
        public string PropertyName { get; set; }
        //public TraceObject Parent { get; set; }
    }
}
