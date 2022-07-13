using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CMS.Context
{
    [Table("CMS.M_CARD")]
    public partial class M_CARD
    {
        [Key]
        public decimal CARD_ID { get; set; }
        [StringLength(20)]
        public string NIK { get; set; }
        [StringLength(20)]
        public string CARDUID { get; set; }
        public string COMMIT_DATE { get; set; }
        public string PROVINSI_CODE { get; set; }
        public string KABKOTA_CODE { get; set; }
        public string INNER { get; set; }
        public string OUTER { get; set; }

        public decimal? CREATEBY { get; set; }

        public decimal? UPDATEBY { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public DateTime? UPDATEDATE { get; set; }
    }
}