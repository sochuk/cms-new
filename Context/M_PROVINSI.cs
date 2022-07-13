using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CMS.Context
{
    [Table("CMS.M_PROVINSI")]
    public partial class M_PROVINSI
    {
        [Key]
        public int ID { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
    }
}