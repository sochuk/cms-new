namespace CMS.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CMS.NOTIFICATION")]
    public partial class NOTIFICATION
    {
        [Key]
        public decimal NOTIF_ID { get; set; }

        [StringLength(200)]
        public string NOTIF_TITLE { get; set; }

        [StringLength(500)]
        public string NOTIF_MESSAGE { get; set; }

        public decimal? NOTIF_TYPE { get; set; }

        public decimal? TO_USER_ID { get; set; }

        public decimal? ISREAD { get; set; }

        public DateTime? CREATEDATE { get; set; }

        [StringLength(3000)]
        public string URL { get; set; }

        public virtual M_USER M_USER { get; set; }
    }
}
