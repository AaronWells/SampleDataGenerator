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
        private LinkedList<TraceObject> _breadCrumb;

        public Generator(IEnumerable<ValueRule> rulePack)
        {
            _rulePack = rulePack;
        }

        public void Populate(object input)
        {
            var propertyExtract = CreatePropertyExtract(input);
            var graph = PrepareDependencyGraph(propertyExtract);

            GenerateObjectHierarchy(input);

            _breadCrumb = new LinkedList<TraceObject>();
            _breadCrumb.AddFirst(new TraceObject
            {
                ObjectToTrace = input,
                PropertyName = string.Empty,
                PropertyTypeName = input.GetType().Name
            });

            DoPopulate();
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
                // find any matching rule
                ValueRule bestMatchingRule = null;
                foreach (var rule in _rulePack)
                {
                    if (propertyMetadata.Matches(rule.Path) &&
                        (bestMatchingRule == null || rule.Path.Length < bestMatchingRule.Path.Length))
                    {
                        bestMatchingRule = rule;
                    }
                }
                if (bestMatchingRule != null && bestMatchingRule.HasDependency)
                {
                    // add dependencies
                    var dependencies =
                        bestMatchingRule.ValueProvider.LookupProperties
                            .Select(d => propertyExtract.Single(p => p.Matches(propertyMetadata.ResolveRelativePath(d))))
                            .ToList();

                    graph.SetDependencies(propertyMetadata, dependencies);
                }
            }
            return graph;
        }

        private void DoPopulate()
        {
            var input = _breadCrumb.Last.Value.ObjectToTrace;

            var type = input.GetType();
            // can't populate non-class types
            if (!type.IsClass)
            {
                throw new ArgumentException(string.Format("cannot populate non-class type: '{0}'", type.FullName));
            }

            var properties = type.GetProperties();

            // filter out 'id' for Complex Objects 
            if (input is ComplexObjectType)
            {
                properties = properties.Where(p => p.Name != "id").ToArray();
            }

            foreach (var property in properties)
            {
                var value =
                    new RuleBaseGenerator(_breadCrumb.Last, _rulePack).Handle(property) ??
                    new StandardGenerator().Handle(property);

                if (property.PropertyType.IsCompositeType())
                {
                    _breadCrumb.AddLast(new TraceObject
                    {
                        ObjectToTrace = value,
                        PropertyName = property.Name,
                        PropertyTypeName = property.PropertyType.Name
                    });
                    DoPopulate();
                    _breadCrumb.RemoveLast();
                }

                input.SetPropertyValue(property.Name, value);

            }
        }

        private static void GenerateObjectHierarchy(object input)
        {
            var type = input.GetType();
            // can't generate hierarchy for non-class types
            if (!type.IsClass)
            {
                throw new ArgumentException(string.Format("cannot populate non-class type: '{0}'", type.FullName));
            }

            var properties = type.GetProperties();

            foreach (var property in properties.Where(p=>p.PropertyType.IsCompositeType()))
            {
                input.SetPropertyValue(property.Name, GetMeA(property.PropertyType));
            }
        }

        private static object GetMeA(Type propertyType)
        {
            var instance = Activator.CreateInstance(propertyType);
            var properties = propertyType.GetProperties(); 
            foreach (var property in properties.Where(p => p.PropertyType.IsCompositeType()))
            {
                instance.SetPropertyValue(property.Name, GetMeA(propertyType));
            }
            return instance;
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
