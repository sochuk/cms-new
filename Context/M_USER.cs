namespace CMS.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CMS.M_USER")]
    public partial class M_USER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public M_USER()
        {
            LOG_USER = new HashSet<LOG_USER>();
            M_TOKEN = new HashSet<M_TOKEN>();
            MESSAGEs = new HashSet<MESSAGE>();
            MESSAGEs1 = new HashSet<MESSAGE>();
            NOTIFICATIONs = new HashSet<NOTIFICATION>();
        }

        [Key]
        public decimal USER_ID { get; set; }

        [StringLength(20)]
        public string USERNAME { get; set; }

        [StringLength(100)]
        public string FULLNAME { get; set; }

        [StringLength(50)]
        public string EMAIL { get; set; }

        [StringLength(50)]
        public string TELEGRAM_ID { get; set; }

        [StringLength(18)]
        public string PHONE { get; set; }

        [StringLength(100)]
        public string PHOTO { get; set; }

        public decimal? GENDER { get; set; }

        public decimal? GROUP_ID { get; set; }

        public decimal? ISACTIVE { get; set; }

        public decimal? ISDELETE { get; set; }

        public decimal? ISREQUIRED_TOKEN { get; set; }

        public decimal? CREATEBY { get; set; }

        public decimal? UPDATEBY { get; set; }

        public decimal? DELETEBY { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public DateTime? UPDATEDATE { get; set; }

        public DateTime? DELETEDATE { get; set; }

        public decimal THEME_ID { get; set; }

        public decimal COMPANY_ID { get; set; }

        [StringLength(4000)]
        public string NOTES { get; set; }

        [StringLength(4000)]
        public string PASSWORD { get; set; }

        public decimal? VENDOR_ID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOG_USER> LOG_USER { get; set; }

        public virtual M_COMPANY M_COMPANY { get; set; }

        public virtual M_GROUP M_GROUP { get; set; }

        public virtual M_SETTING M_SETTING { get; set; }

        public virtual M_THEME M_THEME { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<M_TOKEN> M_TOKEN { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MESSAGE> MESSAGEs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MESSAGE> MESSAGEs1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NOTIFICATION> NOTIFICATIONs { get; set; }
    }
}
