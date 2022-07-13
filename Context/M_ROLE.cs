namespace CMS.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CMS.M_ROLE")]
    public partial class M_ROLE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public M_ROLE()
        {
            M_GROUP = new HashSet<M_GROUP>();
        }

        [Key]
        public decimal ROLE_ID { get; set; }

        [StringLength(50)]
        public string ROLE_NAME { get; set; }

        [StringLength(250)]
        public string ROLE_DESC { get; set; }

        public decimal? ISACTIVE { get; set; }

        public decimal? ISDELETE { get; set; }

        public decimal? ALLOW_CREATE { get; set; }

        public decimal? ALLOW_UPDATE { get; set; }

        public decimal? ALLOW_DELETE { get; set; }

        public decimal? ALLOW_EXPORT { get; set; }

        public decimal? ALLOW_IMPORT { get; set; }

        public decimal? ALLOW_ENABLEDISABLE { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public DateTime? UPDATEDATE { get; set; }

        public DateTime? DELETEDATE { get; set; }

        public decimal? CREATEBY { get; set; }

        public decimal? UPDATEBY { get; set; }

        public decimal? DELETEBY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<M_GROUP> M_GROUP { get; set; }
    }
}
