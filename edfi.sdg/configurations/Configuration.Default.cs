using edfi.sdg.generators;
using edfi.sdg.models;

namespace edfi.sdg.configurations
{
    public partial class Configuration
    {
        public static Configuration DefaultConfiguration
        {
            get
            {
                return new Configuration
                {
                    MaxQueueWrites = 10,
                    NumThreads = 1,
                    WorkQueueName = @".\Private$\edfi.sdg",
                    ValueRules = new ValueRule[]
                    {
                        new ValueRule{Criteria = "Sex", ValueProvider = new DistributedEnumValueProvider<SexType>()},
                        new ValueRule{Criteria = "OldEthnicity", ValueProvider = new DistributedEnumValueProvider<OldEthnicityType>()},
                        new ValueRule{Criteria = "FirstName", ValueProvider = new StatTableValueProvider{LookupProperties = new []{"..Sex"}, DataProvider = new DatabaseStatDataProvider{StatTableName = "GivenName"}}}
                    },
                    WorkItems = new WorkItem[]
                    {
                        new TypeQuantityWorkItem<Student> {QuantitySpecifier = new ConstantQuantity {Quantity = 200}},
                        new PropertyPopulatorWorkItem{ClassFilterRegex = @"^edfi\.sdg\.models\.((Student)|(Parent))$"},
                    }
                };
            }
        }
    }
}
