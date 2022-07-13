namespace CMS.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CMS.M_MODULE")]
    public partial class M_MODULE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public M_MODULE()
        {
            LOG_USER = new HashSet<LOG_USER>();
            M_ACCESS = new HashSet<M_ACCESS>();
            M_MODULE1 = new HashSet<M_MODULE>();
        }

        [Key]
        public decimal MODULE_ID { get; set; }

        [StringLength(25)]
        public string TYPE_CODE { get; set; }

        [StringLength(100)]
        public string MODULE_NAME { get; set; }

        [StringLength(250)]
        public string MODULE_DESC { get; set; }

        [StringLength(100)]
        public string MODULE_TITLE { get; set; }

        public decimal? MODULE_ROOT { get; set; }

        [StringLength(100)]
        public string MODULE_ICON { get; set; }

        [StringLength(250)]
        public string MODULE_URL { get; set; }

        public decimal? MODULE_TYPE { get; set; }

        public decimal? ISACTIVE { get; set; }

        public decimal? ISDELETE { get; set; }

        public decimal? ISVISIBLE { get; set; }

        public decimal? ORDER_NO { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public DateTime? UPDATEDATE { get; set; }

        public DateTime? DELETEDATE { get; set; }

        public decimal? CREATEBY { get; set; }

        public decimal? UPDATEBY { get; set; }

        public decimal? DELETEBY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOG_USER> LOG_USER { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<M_ACCESS> M_ACCESS { get; set; }

        public virtual M_ICON M_ICON { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<M_MODULE> M_MODULE1 { get; set; }

        public virtual M_MODULE M_MODULE2 { get; set; }
    }
}
