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
    
    public partial class LOG_PERSO
    {
        public decimal LOG_ID { get; set; }
        public string CARDUID { get; set; }
        public string CONTROLNUMBER { get; set; }
        public string MANUFACTURERCODE { get; set; }
        public string PERSOSITE { get; set; }
        public Nullable<System.DateTime> PERSO_DATE { get; set; }
        public string ERROR_CODE { get; set; }
        public Nullable<decimal> VENDOR_ID { get; set; }
        public Nullable<decimal> CONTRACT_ID { get; set; }
        public Nullable<decimal> IS_COMMIT { get; set; }
        public string IP_REQUEST { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public Nullable<System.DateTime> UPDATEDATE { get; set; }
        public string IP_ADDRESS { get; set; }
    
        public virtual M_CONTRACT M_CONTRACT { get; set; }
        public virtual M_VENDOR M_VENDOR { get; set; }
    }
}
