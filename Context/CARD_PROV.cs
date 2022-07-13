namespace CMS.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CMS.CARD_PROV")]
    public partial class CARD_PROV
    {
        [Key]
        public decimal PROV { get; set; }

        public decimal? VALUE { get; set; }

        [StringLength(100)]
        public string DESCRIPTION { get; set; }
    }
}