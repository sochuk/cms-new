namespace CMS.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CMS.M_TOKEN")]
    public partial class M_TOKEN
    {
        [Key]
        public decimal TOKEN_ID { get; set; }

        public decimal? USER_ID { get; set; }

        [StringLength(6)]
        public string TOKEN { get; set; }

        public decimal? ISACTIVE { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public virtual M_USER M_USER { get; set; }
    }
}
