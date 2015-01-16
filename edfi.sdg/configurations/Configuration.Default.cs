using EdFi.SampleDataGenerator.Models;
using EdFi.SampleDataGenerator.Quantity;
using EdFi.SampleDataGenerator.Repository;
using EdFi.SampleDataGenerator.ValueProviders;
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
                        new ValueRule{Class = "SexType", PropertySpecifier = "Sex", ValueProvider = new DistributedEnumValueProvider<SexType>()},
                        new ValueRule{Class = "OldEthnicityType", PropertySpecifier = "OldEthnicity", ValueProvider = new DistributedEnumValueProvider<OldEthnicityType>()},
                        new ValueRule{Class = "Name", PropertySpecifier = "FirstName", ValueProvider = new StatTableValueProvider{LookupProperties = new []{"..Sex"}, DataRepository = new DatabaseStatDataRepository {StatTableName = "GivenName"}}},
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
