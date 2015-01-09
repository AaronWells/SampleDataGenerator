using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using edfi.sdg.utility;

namespace edfi.sdg.generators
{
    public class Generator
    {
        public Generator()
        {
        }

        public Generator(Assembly defaultAssembly)
        {
            DefaultAssembly = defaultAssembly;
            RuleSets = new List<RuleSet>();
        }

        public Assembly DefaultAssembly { get; set; }
    
        public List<RuleSet> RuleSets { get; set; }

        public void Populate(object input)
        {
            // could not use this since enum is a value type
/*
            //check for system types
            var type = input.GetType();
            if (type.Namespace == "System")
            {
                throw new ArgumentException(string.Format("cannot populate System type: '{0}'", type.FullName));
            }
            if(type.IsValueType)
            {
                throw new ArgumentException(string.Format("cannot populate Value type: '{0}'", type.FullName));
            }

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                object value;

                //if RuleSet contains trigger for this property:
                var propFullName = type.FullName + "." + property.Name;
                var rule = RuleSets.SingleOrDefault(r => r.RuleTrigger == propFullName);
                if (rule != null)
                {
                    //use the rule
                    Console.WriteLine("check the rules against {0}", rule.RuleName);
                    value = rule.Action();

                }
                else
                    value = GetMeA(property.PropertyType.FullName);

               input.SetPropertyValue(property.Name, value);
            }
*/

        }

        public object GetMeA(string @namespace, string typeName)
        {
            var typeFullName = string.Format("{0}.{1}", @namespace, typeName);
            return GetMeA(typeFullName);
        }

        public object GetMeA(string typeFullName)
        {
            //check for rules:
/*
            if (typeFullName.HasSpecialRule)
            {
                return typeFullName.SpecialRule.Action();
            }
*/

            //check for system types
            if (typeFullName.FirstSegment() == "System")
            {
                return Assembly.GetAssembly(typeof (int))
                    .GetType(typeFullName)
                    .GetDefaultValue();
            }
            // else it is a class type
            var type = DefaultAssembly.GetType(typeFullName);

            var o = Activator.CreateInstance(type);

            // populate object
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                object value;

                //if RuleSet contains trigger for this property:
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

                o.SetPropertyValue(property.Name, value);
            }

            return o;
        }
    }
}
