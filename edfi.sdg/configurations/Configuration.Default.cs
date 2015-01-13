using EdFi.SampleDataGenerator.Models;
using EdFi.SampleDataGenerator.Quantity;
using EdFi.SampleDataGenerator.ValueProvider;
using EdFi.SampleDataGenerator.WorkItems;

namespace EdFi.SampleDataGenerator.Configurations
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
                    ThreadCount = 1,
                    WorkQueueName = @".\Private$\edfi.sdg",
                    ValueRules = new []
                    {
                        new ValueRule{Class = "*", PropertySpecifier = "Sex", ValueProvider = new DistributedEnumValueProvider<SexType>()},
                        new ValueRule{Class = "*", PropertySpecifier = "OldEthnicity", ValueProvider = new DistributedEnumValueProvider<OldEthnicityType>()},
                        new ValueRule{Class = "*", PropertySpecifier = "FirstName", ValueProvider = new StatTableValueProvider{LookupProperties = new []{"..Sex"}, DataValueProvider = new DatabaseStatDataValueProvider {StatTableName = "GivenName"}}},
//                        new ValueRule{Criteria = "FirstName", ValueProvider = new TestValueProvider()}
                    },
                    WorkFlow = new WorkItem[]
                    {
                        new TypeQuantityWorkItem<Student> {QuantitySpecifier = new ConstantQuantity {Quantity = 3}},
                        new PropertyPopulatorWorkItem{ClassFilterRegex = @"^edfi\.sdg\.models\.((Student)|(Parent))$"}
                    }
                };
            }
        }
    }
}
