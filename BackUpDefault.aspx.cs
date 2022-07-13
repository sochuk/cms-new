using DevExpress.Web.Bootstrap;
using CMS.Helper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

namespace CMS
{
    public partial class BackUpDefault : CPanel
    {
        public static DataTable vendor = new DataTable();

        public string seriesHourly = "";
        public string seriesDaily = "";
        public string seriesMonthly = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            var url = Context.Request.Url.Scheme + "://" + Context.Request.Url.DnsSafeHost;
            string fullUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + (HttpContext.Current.Request.ApplicationPath == "/"
                        ? string.Empty : HttpContext.Current.Request.ApplicationPath);
        }
        
        
    }
}