using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edfi.sdg.configurations
{
    using edfi.sdg.generators;

    [Serializable]
    public class ValueRule
    {
        public string Criteria { get; set; }

        public ValueProvider ValueProvider { get; set; }
    }
}
