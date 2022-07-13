namespace CMS.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CMS.M_COMPANY")]
    public partial class M_COMPANY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public M_COMPANY()
        {
            M_USER = new HashSet<M_USER>();
        }

        [Key]
        public decimal COMPANY_ID { get; set; }

        [StringLength(50)]
        public string COMPANY_NAME { get; set; }

        [StringLength(250)]
        public string COMPANY_DESC { get; set; }

        public decimal? ISACTIVE { get; set; }

        public decimal? ISDELETE { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public DateTime? UPDATEDATE { get; set; }

        public DateTime? DELETEDATE { get; set; }

        public decimal? CREATEBY { get; set; }

        public decimal? UPDATEBY { get; set; }

        public decimal? DELETEBY { get; set; }

        public virtual M_CONFIGURATION M_CONFIGURATION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<M_USER> M_USER { get; set; }
    }
}
