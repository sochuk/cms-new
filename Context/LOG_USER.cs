namespace CMS.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CMS.LOG_USER")]
    public partial class LOG_USER
    {
        [Key]
        public decimal LOG_ID { get; set; }

        public DateTime? LOG_DATE { get; set; }

        [StringLength(250)]
        public string LOG_TITLE { get; set; }

        public decimal? LOG_TYPE { get; set; }

        public decimal? USER_ID { get; set; }

        public decimal? MODULE_ID { get; set; }

        [StringLength(32)]
        public string LOCAL_IP { get; set; }

        [StringLength(32)]
        public string REMOTE_IP { get; set; }

        [StringLength(2)]
        public string LOCATION { get; set; }

        [StringLength(25)]
        public string BROWSER { get; set; }

        [StringLength(3000)]
        public string LOG_CONTENT { get; set; }

        public virtual M_MODULE M_MODULE { get; set; }

        public virtual M_USER M_USER { get; set; }
    }
}
