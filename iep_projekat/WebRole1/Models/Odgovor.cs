namespace WebRole1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Odgovor")]
    public partial class Odgovor
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdPit { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdKor { get; set; }

        public DateTime VrSlanja { get; set; }

        public int IdKan { get; set; }

        public int IdPon { get; set; }

        public virtual Kanal Kanal { get; set; }

        public virtual Korisnik Korisnik { get; set; }

        public virtual Pitanje Pitanje { get; set; }

        public virtual PonudjeniOdg PonudjeniOdg { get; set; }
    }
}
