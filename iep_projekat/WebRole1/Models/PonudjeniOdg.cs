namespace WebRole1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PonudjeniOdg")]
    public partial class PonudjeniOdg
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PonudjeniOdg()
        {
            Odgovors = new HashSet<Odgovor>();
        }

        public int IdPit { get; set; }

        [Key]
        public int IdPon { get; set; }

        [Required]
        [StringLength(100)]
        public string Sadrzaj { get; set; }

        public int RedniBr { get; set; }

        public bool Tacan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Odgovor> Odgovors { get; set; }

        public virtual Pitanje Pitanje { get; set; }
    }
}
