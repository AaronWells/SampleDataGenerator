namespace edfi.sdg.entity
{
    using System.ComponentModel.DataAnnotations;

    public class StatTemplate
    {
        public long Id { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public string Attribute { get; set; }

        public decimal Prop100k { get; set; }
    }
}
