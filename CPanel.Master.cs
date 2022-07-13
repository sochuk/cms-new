using System;
using System.Web;
using System.Web.UI;
using CMS.Management.Model;

namespace CMS
{
    public partial class CPanelMaster : MasterPage
    {        
        protected void Page_PreInit(object sender, EventArgs e)
        {
            try
            {
                string _default = HttpContext.Current.User.Identity.Get_Theme().ToString();
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if(_default != string.Empty)
                        this.Page.MasterPageFile = "~/Themes/" + HttpContext.Current.User.Identity.Get_Theme() + "/" + HttpContext.Current.User.Identity.Get_Theme() + ".master";
                    else
                        this.Page.MasterPageFile = "~/Themes/Material/Material.master";
                }
                else
                {
                    this.Page.MasterPageFile = "~/Site.master";
                }                                    
            }
            catch (Exception ex)
            {
                ex.Message.ToString();

                //Logout if theme doesnt exist
                Response.Redirect("~/account/logout");
            }
        }
    }
}