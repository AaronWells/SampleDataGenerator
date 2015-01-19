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
                        new ValueRule{Class = "Student", PropertySpecifier = "Sex", ValueProvider = new DistributedEnumValueProvider<SexType>()},
                        new ValueRule{Class = "Student", PropertySpecifier = "OldEthnicity", ValueProvider = new DistributedEnumValueProvider<OldEthnicityType>()},
                        new ValueRule{Class = "Student", PropertySpecifier = "Citizenship.CitizenshipStatus", ValueProvider = new DistributedEnumValueProvider<CitizenshipStatusType>()},
                        new ValueRule{Class = "Name", PropertySpecifier = "FirstName", ValueProvider = new StatTableValueProvider{LookupProperties = new []{"{parent}.Sex"}, DataRepository = new DatabaseStatDataRepository {StatTableName = "GivenName"}}},
                        new ValueRule{Class = "Name", PropertySpecifier = "LastSurname", ValueProvider = new StatTableValueProvider{LookupProperties = new []{"{parent}.OldEthnicity", }, DataRepository = new DatabaseStatDataRepository {StatTableName = "FamilyName"}}},
                    },
                    WorkFlow = new WorkItem[]
                    {
                        new TypeQuantityWorkItem<Student> {QuantitySpecifier = new ConstantQuantity {Quantity = 10}},
                        new PropertyPopulatorWorkItem{ClassFilterRegex = @"^EdFi\.SampleDataGenerator\.Models\.((Student)|(Parent))$"}
                    }
                };
            }
        }
    }
}
