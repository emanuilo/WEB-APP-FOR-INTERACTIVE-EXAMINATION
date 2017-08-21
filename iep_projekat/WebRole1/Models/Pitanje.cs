namespace WebRole1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("Pitanje")]
    public partial class Pitanje
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pitanje()
        {
            Odgovors = new HashSet<Odgovor>();
            PonudjeniOdgs = new HashSet<PonudjeniOdg>();
        }

        [Key]
        public int IdPit { get; set; }

        [StringLength(80)]
        [Required]
        public string Naslov { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Tekst { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageToUpload { get; set; }

        public byte[] Slika { get; set; }

        [Display(Name = "Vreme Pravljenja")]
        public DateTime VrPravljenja { get; set; }

        [Display(Name = "Vreme Zakljucavanja")]
        public DateTime? VrPoslZaklj { get; set; }

        public bool Zakljucano { get; set; }

        public int IdKor { get; set; }

        public int? IdKan { get; set; }

        public virtual Kanal Kanal { get; set; }

        public virtual Klon Klon { get; set; }

        public virtual Korisnik Korisnik { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Odgovor> Odgovors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PonudjeniOdg> PonudjeniOdgs { get; set; }
    }
}
