namespace edfi.sdg.data
{
    using System.Data.Entity;

    public partial class DataModel : DbContext
    {
        public DataModel()
            : base("name=DataModel")
        {
        }

        public virtual DbSet<ComplexObject> ComplexObjects { get; set; }
        public virtual DbSet<ComplexObjectClass> ComplexObjectClasses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ComplexObject>()
                .Property(e => e.Id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ComplexObjectClass>()
                .HasMany(e => e.ComplexObjects)
                .WithRequired(e => e.ComplexObjectClass)
                .WillCascadeOnDelete(false);
        }
    }
}
