namespace CMS.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CMS.M_ICON")]
    public partial class M_ICON
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public M_ICON()
        {
            M_MODULE = new HashSet<M_MODULE>();
        }

        [Key]
        [StringLength(50)]
        public string ICON_CODE { get; set; }

        [StringLength(250)]
        public string ICON_NAME { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<M_MODULE> M_MODULE { get; set; }
    }
}
