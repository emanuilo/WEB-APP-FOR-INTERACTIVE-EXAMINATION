namespace WebRole1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("Klon")]
    public partial class Klon
    {
        [Required]
        [StringLength(80)]
        public string Naslov { get; set; }

        [Column(TypeName = "text")]
        public string Tekst { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageToUpload { get; set; }

        public byte[] Slika { get; set; }

        [Display(Name = "Vreme Pravljenja")]
        public DateTime? VrPravljenja { get; set; }

        [Display(Name = "Vreme Zakljucavanja")]
        public DateTime? VrPoslZaklj { get; set; }

        public bool? Zakljucano { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdPit { get; set; }

        public virtual Pitanje Pitanje { get; set; }
    }
}
