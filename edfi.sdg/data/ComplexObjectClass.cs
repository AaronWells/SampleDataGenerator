namespace edfi.sdg.data
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ComplexObjectClass")]
    public partial class ComplexObjectClass
    {
        public ComplexObjectClass()
        {
            ComplexObjects = new HashSet<ComplexObject>();
        }

        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<ComplexObject> ComplexObjects { get; set; }
    }
}
