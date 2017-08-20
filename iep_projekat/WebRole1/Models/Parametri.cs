namespace WebRole1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Parametri")]
    public partial class Parametri
    {
        [Key]
        public int IdPar { get; set; }

        public int? K { get; set; }

        public int? M { get; set; }

        public int? E { get; set; }

        public int? S { get; set; }

        public int? G { get; set; }

        public int? P { get; set; }
    }
}
