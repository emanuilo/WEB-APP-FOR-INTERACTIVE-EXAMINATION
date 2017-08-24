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
        public int IdPit { get; set; }

        [Key]
        public int IdPon { get; set; }

        [Required]
        [StringLength(200)]
        public string Sadrzaj { get; set; }

        public int RedniBr { get; set; }

        public bool Tacan { get; set; }

        public virtual Pitanje Pitanje { get; set; }
    }
}
