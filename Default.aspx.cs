using DevExpress.Web.Bootstrap;
using CMS.Helper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using CMS.Context;
using System.Threading.Tasks;
using CMS.Management.Model;

namespace CMS
{
    public partial class Default : CPanel
    {
        public List<CARD_SUMMARY> summary = new List<CARD_SUMMARY>();
        public DataTable groupByProvince = new DataTable();
        protected async void Page_Load(object sender, EventArgs e)
        {
            summary = new List<CARD_SUMMARY>();
            try
            {
                string token = await getTokenCard();
                using (var context = new CMSContext())
                {
                    summary = (from a in context.CARD_SUMMARY
                               select a).ToList();

                    groupByProvince = Database.getDataTable("SELECT * FROM CARD_PROV");

                }
                var url = Context.Request.Url.Scheme + "://" + Context.Request.Url.DnsSafeHost;
                string fullUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + (HttpContext.Current.Request.ApplicationPath == "/"
                            ? string.Empty : HttpContext.Current.Request.ApplicationPath);
            }
            catch (Exception ex) { }

        }

        private async Task<string> getTokenCard()
        {
            string res = await WebAPI.GetAuthorizeToken();
            return res;
        }

        //private async Task<M_CardSummary> getCardSummary(string token)
        //{
        //    var result = new M_CardSummary();


        //}

    }
}