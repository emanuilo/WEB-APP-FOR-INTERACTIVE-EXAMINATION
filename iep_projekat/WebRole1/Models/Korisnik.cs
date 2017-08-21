namespace WebRole1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Korisnik")]
    public partial class Korisnik
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Korisnik()
        {
            Kanals = new HashSet<Kanal>();
            Odgovors = new HashSet<Odgovor>();
            Narudzbinas = new HashSet<Narudzbina>();
            Pratis = new HashSet<Prati>();
            Pitanjes = new HashSet<Pitanje>();
        }

        [Key]
        public int IdKor { get; set; }

        [Required]
        [StringLength(50)]
        public string Ime { get; set; }

        [Required]
        [StringLength(50)]
        public string Prezime { get; set; }

        [System.ComponentModel.DataAnnotations.EmailAddressAttribute(ErrorMessage = "Invalid Email Address")]
        [Required]
        [StringLength(80)]
        public string Email { get; set; }

        [Required]
        [StringLength(150)]
        public string Lozinka { get; set; }

        [NotMapped]
        [System.ComponentModel.DataAnnotations.CompareAttribute("Lozinka", ErrorMessage = "Unos se ne poklapa")]
        [Display(Name = "Potvrda lozinke")]
        public string PotvrdaLozinke { get; set; }

        public int BrTokena { get; set; }
                
        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string Uloga { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kanal> Kanals { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Odgovor> Odgovors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Narudzbina> Narudzbinas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Prati> Pratis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pitanje> Pitanjes { get; set; }
    }
}
