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
        }

        [Key]
        public int IdKan { get; set; }

        [Required]
        [StringLength(50)]
        public string Naziv { get; set; }

        public DateTime VrOtvaranja { get; set; }

        [Required]
        [StringLength(50)]
        public string Lozinka { get; set; }

        public bool Otvoren { get; set; }

        public bool? VrOgranicen { get; set; }

        public int? IntervalTrajanja { get; set; }

        public int IdKor { get; set; }

        public virtual Korisnik Korisnik { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pitanje> Pitanjes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Odgovor> Odgovors { get; set; }
    }
}
