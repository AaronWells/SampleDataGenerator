namespace edfi.sdg.data
{
    using System.Data.Entity;

    public partial class DataModel : DbContext
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
