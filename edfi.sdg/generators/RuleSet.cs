using System;

namespace edfi.sdg.generators
{
    public class RuleSet
    {
        public string RuleName { get; set; }
        public string RuleTrigger { get; set; }
        public Func<object> Action { get; set; }
    }
}