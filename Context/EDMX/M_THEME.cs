//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CMS.Context.EDMX
{
    using System;
    using System.Collections.Generic;
    
    public partial class M_THEME
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public M_THEME()
        {
            this.M_USER = new HashSet<M_USER>();
        }
    
        public decimal THEME_ID { get; set; }
        public string THEME_NAME { get; set; }
        public string THEME_DESC { get; set; }
        public string THEME_LOCATION { get; set; }
        public Nullable<decimal> ISACTIVE { get; set; }
        public Nullable<decimal> ISDELETE { get; set; }
        public Nullable<decimal> CREATEBY { get; set; }
        public Nullable<decimal> UPDATEBY { get; set; }
        public Nullable<decimal> DELETEBY { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public Nullable<System.DateTime> UPDATEDATE { get; set; }
        public Nullable<System.DateTime> DELETEDATE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<M_USER> M_USER { get; set; }
    }
}
