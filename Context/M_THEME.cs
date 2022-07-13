namespace CMS.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CMS.M_THEME")]
    public partial class M_THEME
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public M_THEME()
        {
            M_USER = new HashSet<M_USER>();
        }

        [Key]
        public decimal THEME_ID { get; set; }

        [StringLength(50)]
        public string THEME_NAME { get; set; }

        [StringLength(500)]
        public string THEME_DESC { get; set; }

        [StringLength(500)]
        public string THEME_LOCATION { get; set; }

        public decimal? ISACTIVE { get; set; }

        public decimal? ISDELETE { get; set; }

        public decimal? CREATEBY { get; set; }

        public decimal? UPDATEBY { get; set; }

        public decimal? DELETEBY { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public DateTime? UPDATEDATE { get; set; }

        public DateTime? DELETEDATE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<M_USER> M_USER { get; set; }
    }
}
