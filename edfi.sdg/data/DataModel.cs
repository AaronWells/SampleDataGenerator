using System.Data.Entity;

namespace EdFi.SampleDataGenerator.Data
{
    public class DataModel : DbContext
    {
        public DataModel()
            : base("name=DataModel")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
