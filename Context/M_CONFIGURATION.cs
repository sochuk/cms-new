namespace CMS.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CMS.M_CONFIGURATION")]
    public partial class M_CONFIGURATION
    {
        [Key]
        public decimal COMPANY_ID { get; set; }

        [StringLength(250)]
        public string TELEGRAM_API { get; set; }

        [StringLength(100)]
        public string SMTP_MAIL { get; set; }

        [StringLength(100)]
        public string SMTP_SERVER { get; set; }

        public decimal? SMTP_PORT { get; set; }

        public decimal? CREATEBY { get; set; }

        public decimal? UPDATEBY { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public DateTime? UPDATEDATE { get; set; }

        [StringLength(3000)]
        public string SMTP_PASSWORD { get; set; }

        public virtual M_COMPANY M_COMPANY { get; set; }
    }
}
