using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using edfi.sdg.utility;

namespace edfi.sdg.generators
{
    public class Generator
    {
        private readonly Assembly _assembly;

        public Generator(Assembly assembly)
        {
            _assembly = assembly;
            RuleSets = new List<RuleSet>();
        }

        public List<RuleSet> RuleSets { get; set; }

        public static void Populate(object input)
        {
            throw new NotImplementedException();
        }

        public object GetMeA(string @namespace, string typeName)
        {
            var typeFullName = string.Format("{0}.{1}", @namespace, typeName);
            return GetMeA(typeFullName);
        }

        public object GetMeA(string typeFullName)
        {
            //check for system types
            if (typeFullName.FirstSegment() == "System")
            {
                return null;
            }

            var type = _assembly.GetType(typeFullName);


            var o = Activator.CreateInstance(type);

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                object value;

                //if ruleset contains trigger for this property:
                var propFullName = typeFullName + "." + property.Name;
                var rule = RuleSets.SingleOrDefault(r => r.RuleTrigger == propFullName);
                if (rule != null)
                {
                    //use the rule
                    Console.WriteLine("check the rules against {0}", rule.RuleName);
                    value = rule.Action();

                }
                else
                    value = GetMeA(property.PropertyType.FullName);

                o.SetValue(property.Name, value);
            }

            return o;
        }

        private static IEnumerable<PropertyInfo> GetSystemProperties(Type type)
        {
            return type.GetProperties().Where(p => p.PropertyType.Namespace == "System");
        }

        private static IEnumerable<PropertyInfo> GetCompositeProperties(Type type)
        {
            return type.GetProperties().Where(
                p => !p.PropertyType.IsArray && p.PropertyType.Namespace != "System" && p.PropertyType.IsClass);
        }
    }
}
