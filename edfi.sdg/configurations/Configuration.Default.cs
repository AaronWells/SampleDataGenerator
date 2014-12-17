namespace edfi.sdg.configurations
{
    using edfi.sdg.generators;
    using edfi.sdg.models;

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
                               Generators =
                                   new Generator[]
                                       {
                                           new TypeQuantityGenerator<Student>{ QuantitySpecifier = new NormalQuantity{Min = 150, Max = 250} },
                                           new DistributedEnumValueGenerator<SexType>{ Property = "Student.Sex" },
                                           new DistributedEnumValueGenerator<OldEthnicityType>{ Property = "Student.OldEthnicity" },
                                       }
                           };
            }
        }
    }
}
