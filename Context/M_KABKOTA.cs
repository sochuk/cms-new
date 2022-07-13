using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CMS.Context
{
    [Table("CMS.M_KABKOTA")]
    public partial class M_KABKOTA
    {
        public int ID { get; set; }
        public int PROVINSI_ID { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }      
        public string TYPE { get; set; }
    }
}