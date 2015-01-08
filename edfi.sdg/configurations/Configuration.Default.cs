using edfi.sdg.generators;
using edfi.sdg.models;

namespace edfi.sdg.configurations
{
    using System.Collections.Generic;

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
                        new ValueRule{Criteria = "Sex", ValueProvider = new SampleValueProvider{MyValue = "foobar"} }
                    },
                    WorkItems = new WorkItem[]
                    {
                        new TypeQuantityWorkItem<Student> {QuantitySpecifier = new ConstantQuantity {Quantity = 200}},
                        new PropertyPopulatorWorkItem{Classes = new string[]{"Student"}},

                        new DistributedEnumValueProvider<SexType> {Property = "Student.Sex"},
                        new DistributedEnumValueProvider<OldEthnicityType> {Property = "Student.OldEthnicity"},

//                        new StatTableValueGenerator {PropertyToSet = "Name", PropertiesToLook = new[] {"Student.OldEthnicity", "Student.Sex"}, DataProvider = new DatabaseStatDataProvider{ StatTableName = "GivenName" }},

/*
 * to do composition
 * one way
                        new StatTableValueGenerator {PropertyToSet = "Name", PropertiesToLook = new[] {"Student.OldEthnicity", "Student.Sex"}, DataProvider = new DatabaseStatDataProvider{ StatTableName = "FamilyName" }, Action = "Append" },
* the other way
                        new FullNameGenerator
                        {
                            PropertyToSet = "Name", 
                            Format = "{1}, {0}"
                            new StatTableValueGenerator {PropertyToSet = "Name", PropertiesToLook = new[] {"Student.OldEthnicity", "Student.Sex"}, DataProvider = new DatabaseStatDataProvider{ StatTableName = "GivenName" }},
                            new StatTableValueGenerator {PropertyToSet = "Name", PropertiesToLook = new[] {"Student.OldEthnicity", "Student.Sex"}, DataProvider = new DatabaseStatDataProvider{ StatTableName = "FamilyName" }},
                        }
*/
                    }
                };
            }
        }
    }
}
