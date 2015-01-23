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
                    ThreadCount = 8,
                    WorkQueueName = @".\Private$\edfi.sdg",
                    ValueRules = new []
                    {
                        new ValueRule{Class = "Student", PropertySpecifier = "Sex", ValueProvider = new DistributedEnumValueProvider<SexType>()},
                        new ValueRule{Class = "Student", PropertySpecifier = "OldEthnicity", ValueProvider = new DistributedEnumValueProvider<OldEthnicityType>()},
                        new ValueRule{Class = "Student", PropertySpecifier = "Citizenship.CitizenshipStatus", ValueProvider = new DistributedEnumValueProvider<CitizenshipStatusType>()},
                        new ValueRule{Class = "Student", PropertySpecifier = "Name.FirstName", ValueProvider = new StatTableValueProvider{LookupProperties = new []{"{parent}.Sex"}, DataRepository = new DatabaseStatDataRepository {StatTableName = "GivenName"}}},
                        new ValueRule{Class = "Student", PropertySpecifier = "Name.LastSurname", ValueProvider = new StatTableValueProvider{LookupProperties = new []{"{parent}.OldEthnicity", }, DataRepository = new DatabaseStatDataRepository {StatTableName = "FamilyName"}}},
                        new ValueRule{Class = "Parent", PropertySpecifier = "Sex", ValueProvider = new DistributedEnumValueProvider<SexType>()},
//                        new ValueRule{Class = "Parent", PropertySpecifier = "PersonUniqueStateId", ValueProvider = new DistributedEnumValueProvider<StateAbbreviationType>()},
                        new ValueRule{Class = "Parent", PropertySpecifier = "Name.FirstName", ValueProvider = new StatTableValueProvider{LookupProperties = new []{"{parent}.Sex"}, DataRepository = new DatabaseStatDataRepository {StatTableName = "GivenName"}}},
                        new ValueRule{Class = "Parent", PropertySpecifier = "Name.LastSurname", ValueProvider = new StatTableValueProvider{LookupProperties = new string[0], DataRepository = new DatabaseStatDataRepository {StatTableName = "FamilyName"}}},
//                        new ValueRule{Class = "Parent", PropertySpecifier = "ElectronicMail", ValueProvider = new EmailAddressValueProvider{LookupProperties = new []{"Name.FirstName", "Name.LastSurname"} }},
                        new ValueRule{Class = "Parent", PropertySpecifier = "LoginId", ValueProvider = new LoginNameValueProvider{LookupProperties = new []{"Name.FirstName", "Name.LastSurname"} }},
                        new ValueRule{Class = "StudentParentAssociation", PropertySpecifier = "Relation", ValueProvider = new DistributedEnumValueProvider<RelationType>()},
                    },
                    WorkFlow = new WorkItem[]
                    {
                        new CreateInstanceWorkItem
                        {
                            CreatedType = typeof(Student).FullName, 
                            QuantitySpecifier = new ConstantQuantity {Quantity = 20}
                        },
                        new PropertyPopulatorWorkItem
                        {
                            ClassFilterRegex = @"^EdFi\.SampleDataGenerator\.Models\.(Student)$"
                        },
                        new CreateInstanceWorkItem
                        {
                            ClassFilterRegex = @"^EdFi\.SampleDataGenerator\.Models\.Student$",
                            CreatedType = typeof(StudentParentAssociation).FullName, 
                            QuantitySpecifier = new ChiQuantity{Max=6, Min=1},
                        },
                        new CreateInstanceWorkItem
                        {
                            ClassFilterRegex = @"^EdFi\.SampleDataGenerator\.Models\.StudentParentAssociation",
                            CreatedType = typeof(Parent).FullName, 
                            QuantitySpecifier = new ConstantQuantity{Quantity = 1},
                        },
                        new PropertyPopulatorWorkItem
                        {
                            ClassFilterRegex = @"^EdFi\.SampleDataGenerator\.Models\.(Parent)$"
                        },
                        //new PropertyPopulatorWorkItem
                        //{
                        //    ClassFilterRegex = @"^EdFi\.SampleDataGenerator\.Models\.(StudentParentAssociation)$"
                        //},
                    }
                };
            }
        }
    }
}
