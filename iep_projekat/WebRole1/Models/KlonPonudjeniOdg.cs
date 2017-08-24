namespace WebRole1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KlonPonudjeniOdg")]
    public partial class KlonPonudjeniOdg
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KlonPonudjeniOdg()
        {
            Odgovors = new HashSet<Odgovor>();
        }

        [Key]
        public int IdKPO { get; set; }

        [Required]
        [StringLength(200)]
        public string Sadrzaj { get; set; }

        public int RedniBr { get; set; }

        public bool Tacan { get; set; }

        public int IdKlo { get; set; }

        public virtual Klon Klon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Odgovor> Odgovors { get; set; }
    }
}
