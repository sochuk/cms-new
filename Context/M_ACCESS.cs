namespace CMS.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CMS.M_ACCESS")]
    public partial class M_ACCESS
    {
        [Key]
        [Column(Order = 0)]
        public decimal MODULE_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal GROUP_ID { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public virtual M_GROUP M_GROUP { get; set; }

        public virtual M_MODULE M_MODULE { get; set; }
    }
}
