using Microsoft.Owin.Security;
using CMS.Management.Model;
using System;
using System.Web;

namespace CMS.Account
{
    public partial class Logout : System.Web.UI.Page
    {
        private IAuthenticationManager AuthenticationManager => HttpContext.Current.GetOwinContext().Authentication;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                M_User.Logout();                
                AuthenticationManager.SignOut();                
            }
            Response.Redirect("~/");
        }
    }
}