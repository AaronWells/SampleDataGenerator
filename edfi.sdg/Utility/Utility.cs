using System.Collections.Generic;
using EdFi.SampleDataGenerator.Generators;

namespace EdFi.SampleDataGenerator.Utility
{
    public static class Helper
    {
        public static List<string> RuleMatchPossibilities(LinkedListNode<TraceObject> parentNode, string chainTrailer)
        {
            if (parentNode == null) return new List<string>();

            var list = new List<string>();
            var possibleChain = string.Format("{0}::{1}", parentNode.Value.PropertyTypeName, chainTrailer);
            list.Add(possibleChain);

            var anotherPossibility = string.Format("{0}.{1}", parentNode.Value.PropertyName, chainTrailer);
            list.AddRange(RuleMatchPossibilities(parentNode.Previous, anotherPossibility));
            return list;
        }
    }
}