using CMS.Helper;
using CMS.Management.Model;
using System;
using System.Web;

namespace CMS.Account
{
    public partial class Lock : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string returnurl = Request.QueryString["returnurl"] ?? "~/";
            Page.Title = "Lock Screen";
            HttpContext.Current.Session["M_Module"] = null;

            if (!IsPostBack)
            {
                HttpContext.Current.Session["error"] = null;
                if (User.Identity.IsAuthenticated)
                {
                    HttpContext.Current.User.Identity.UpdateClaim(UserClaim.IsLocked, "true");
                }
                else
                {
                    Response.Redirect("~/");
                }
            }
            else
            {
                string password = Crypto.EncryptPassword(txtPassword.Text.Trim());
                if (Validate_Login(password))
                {
                    HttpContext.Current.User.Identity.UpdateClaim(UserClaim.IsLocked, "false");
                    Response.Redirect(returnurl);
                }
                else
                {
                    HttpContext.Current.Session["error"] = "Invalid password";
                }
            }            
        }

        private bool Validate_Login(string value)
        {
            string passwrod = User.Identity.Get_HashPassword();
            if(passwrod == value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}