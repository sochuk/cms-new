using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Security.Principal;

namespace CMS.Management.Model
{
    public static class Themes
    {
        public static string get_ImageDir()
        {
            string theme_location = HttpContext.Current.User.Identity.Get_Theme();
            return "Theme/" + theme_location + "/images/";
        }
    }
}