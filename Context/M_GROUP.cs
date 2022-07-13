namespace CMS.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CMS.M_GROUP")]
    public partial class M_GROUP
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public M_GROUP()
        {
            M_ACCESS = new HashSet<M_ACCESS>();
            M_USER = new HashSet<M_USER>();
        }

        [Key]
        public decimal GROUP_ID { get; set; }

        [StringLength(200)]
        public string GROUP_NAME { get; set; }

        [StringLength(500)]
        public string GROUP_DESC { get; set; }

        public decimal? ROLE_ID { get; set; }

        public decimal? ISACTIVE { get; set; }

        public decimal? ISDELETE { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public DateTime? UPDATEDATE { get; set; }

        public decimal? CREATEBY { get; set; }

        public decimal? UPDATEBY { get; set; }

        public decimal? DELETEBY { get; set; }

        public DateTime? DELETEDATE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<M_ACCESS> M_ACCESS { get; set; }

        public virtual M_ROLE M_ROLE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<M_USER> M_USER { get; set; }
    }
}
