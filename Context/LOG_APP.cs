namespace CMS.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CMS.LOG_APP")]
    public partial class LOG_APP
    {
        [Key]
        public decimal LOG_ID { get; set; }

        [StringLength(100)]
        public string LOG_TITLE { get; set; }

        [StringLength(2000)]
        public string LOG_CONTENT { get; set; }

        [StringLength(20)]
        public string BROWSER { get; set; }

        public DateTime? CREATEDATE { get; set; }
    }
}
