namespace CMS.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CMS.CARD_SUMMARY")]
    public partial class CARD_SUMMARY
    {
        [Key]
        [StringLength(20)]
        public string ITEM { get; set; }

        public decimal? VALUE { get; set; }

        [StringLength(100)]
        public string DESCRIPTION { get; set; }

        [StringLength(20)]
        public string TITLE { get; set; }
    }
}