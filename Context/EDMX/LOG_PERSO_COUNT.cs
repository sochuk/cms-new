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
    
    public partial class LOG_PERSO_COUNT
    {
        public decimal VENDOR_ID { get; set; }
        public decimal CONTRACT_ID { get; set; }
        public Nullable<decimal> CURRENT_HIT { get; set; }
        public Nullable<decimal> SUCCESS_HIT { get; set; }
        public Nullable<decimal> FAIL_HIT { get; set; }
    
        public virtual M_CONTRACT M_CONTRACT { get; set; }
        public virtual M_VENDOR M_VENDOR { get; set; }
    }
}
