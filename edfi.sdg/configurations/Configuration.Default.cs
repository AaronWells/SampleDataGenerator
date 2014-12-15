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
                               MaxQueueWrites = 500,
                               NumThreads = 4,
                               WorkQueueName = @".\Private$\edfi.sdg",
                               Generators =
                                   new Generator[]
                                       {
                                           new TypeQuantityGenerator<Student>{ QuantitySpecifier = new ConstantQuantity{Quantity = 2500000}},
                                           new DistributedEnumValueGenerator<SexType>{ Property = "Sex" },
                                           new DistributedEnumValueGenerator<OldEthnicityType>{ Property = "OldEthnicity" },
                                       }
                           };
            }
        }
    }
}
