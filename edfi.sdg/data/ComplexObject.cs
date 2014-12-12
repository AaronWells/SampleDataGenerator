namespace edfi.sdg.data
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ComplexObject")]
    public partial class ComplexObject
    {
        [StringLength(10)]
        public string Id { get; set; }

        public long ComplexObjectClassId { get; set; }

        [Column(TypeName = "xml")]
        [Required]
        public string Xml { get; set; }

        public virtual ComplexObjectClass ComplexObjectClass { get; set; }
    }
}
