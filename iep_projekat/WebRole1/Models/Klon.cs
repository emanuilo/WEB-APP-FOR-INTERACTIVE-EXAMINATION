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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Klon()
        {
            Odgovors = new HashSet<Odgovor>();
            KlonPonudjeniOdgs = new HashSet<KlonPonudjeniOdg>();
        }

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

        public bool Zakljucano { get; set; }

        public int IdPit { get; set; }

        [Key]
        public int IdKlo { get; set; }

        public int? IdKan { get; set; }

        public virtual Kanal Kanal { get; set; }

        public virtual Pitanje Pitanje { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Odgovor> Odgovors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KlonPonudjeniOdg> KlonPonudjeniOdgs { get; set; }
    }
}
