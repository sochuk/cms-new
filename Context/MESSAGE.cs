namespace CMS.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CMS.MESSAGE")]
    public partial class MESSAGE
    {
        public decimal? USER_FROM { get; set; }

        public decimal? USER_TO { get; set; }

        public decimal? ISREAD { get; set; }

        public DateTime? CREATEDATE { get; set; }

        [StringLength(3000)]
        public string MESSAGE_CONTENT { get; set; }

        [Key]
        public decimal MESSAGE_ID { get; set; }

        public virtual M_USER M_USER { get; set; }

        public virtual M_USER M_USER1 { get; set; }
    }
}
