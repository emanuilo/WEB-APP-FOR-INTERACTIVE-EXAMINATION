namespace WebRole1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Narudzbina")]
    public partial class Narudzbina
    {
        [Key]
        public int IdNar { get; set; }

        public int BrTokena { get; set; }

        public int Cena { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        public int IdKor { get; set; }

        public virtual Korisnik Korisnik { get; set; }
    }
}
