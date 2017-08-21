namespace WebRole1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Prati")]
    public partial class Prati
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdKor { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdKan { get; set; }

        public DateTime? VrPracenja { get; set; }

        public virtual Kanal Kanal { get; set; }

        public virtual Korisnik Korisnik { get; set; }
    }
}
