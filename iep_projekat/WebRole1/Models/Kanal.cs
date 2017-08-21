namespace WebRole1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kanal")]
    public partial class Kanal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kanal()
        {
            Pitanjes = new HashSet<Pitanje>();
            Odgovors = new HashSet<Odgovor>();
            Pratis = new HashSet<Prati>();
        }

        [Key]
        public int IdKan { get; set; }

        [Required]
        [StringLength(50)]
        public string Naziv { get; set; }

        [Display(Name = "Vreme Otvaranja")]
        public DateTime VrOtvaranja { get; set; }

        [Display(Name = "Vreme Zatvaranja")]
        public DateTime? VrZatvaranja { get; set; }

        [Required]
        [StringLength(50)]
        public string Lozinka { get; set; }

        public bool Zatvoren { get; set; }

        [Display(Name = "Vremenski Ogranicen")]
        public bool VrOgranicen { get; set; }

        public int? IntervalTrajanja { get; set; }

        public int IdKor { get; set; }

        public virtual Korisnik Korisnik { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pitanje> Pitanjes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Odgovor> Odgovors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Prati> Pratis { get; set; }
    }
}
