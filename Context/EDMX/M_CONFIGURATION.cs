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
    
    public partial class M_CONFIGURATION
    {
        public decimal COMPANY_ID { get; set; }
        public string TELEGRAM_API { get; set; }
        public string SMTP_MAIL { get; set; }
        public string SMTP_SERVER { get; set; }
        public Nullable<decimal> SMTP_PORT { get; set; }
        public Nullable<decimal> CREATEBY { get; set; }
        public Nullable<decimal> UPDATEBY { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public Nullable<System.DateTime> UPDATEDATE { get; set; }
        public string SMTP_PASSWORD { get; set; }
    
        public virtual M_COMPANY M_COMPANY { get; set; }
    }
}
