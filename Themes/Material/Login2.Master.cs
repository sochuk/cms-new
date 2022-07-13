using DevExpress.Web;
using Microsoft.AspNet.FriendlyUrls;
using CMS.Helper;
using CMS.Management.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.Themes.Material
{
    public partial class Login2 : SiteMaster
    {
        public string background_image = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            background_image = "<style type=\"text/css\">";
            background_image += ".auth-page.lock-full-bg, .auth-page.login-bg {";
            background_image += "background: url(\"" + Files.getRandomFile("~/themes/material/images/wallpaper/", "*.jpg") + "\") !important;";
            background_image += "background-size: cover;";
            background_image += "}";
            background_image += "</style>";


        }
    }
}