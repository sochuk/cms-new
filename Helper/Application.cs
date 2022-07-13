using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CMS.Helper
{
    public static class Application
    {
        public static string GetConfigurationValue(string ConfigurationName)
        {
            var val = ConfigurationManager.AppSettings[ConfigurationName];
            return val;
        }

        public static string GetConnectionString(string ConnectionName)
        {
            return ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString;
        }

        public static bool IsDevelopment()
        {
            var val = ConfigurationManager.AppSettings["Development"];
            if (val == null)
            {
                val = "false";
            }
            return val == "true";
        }

        public static string Name(this HttpApplicationState httpApplicationState)
        {
            var val = ConfigurationManager.AppSettings["Application_Name"];
            if (val == null | val == string.Empty | val == "")
            {
                val = null;
            }
            return val;
        }

        public static string Version()
        {
            var val = ConfigurationManager.AppSettings["Application_Version"];
            if (val == null | val == string.Empty | val == "")
            {
                val = null;
            }
            return val;
        }

        public static string Host()
        {
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            string path = HttpContext.Current.Request.Url.AbsolutePath;
            string host = HttpContext.Current.Request.Url.Host;
            return host;
        }

        public static string GetUrl()
        {
            string url = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + (HttpContext.Current.Request.ApplicationPath == "/" ? string.Empty : HttpContext.Current.Request.ApplicationPath);
            return url; 
        }

        public static string GetPath()
        {
            string url = HttpContext.Current.Request.ApplicationPath == "/" ? string.Empty : HttpContext.Current.Request.ApplicationPath;
            return url;
        }

        public static string GetHost()
        {
            string url = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            return url;
        }

        public static string GetIPAddress()
        {
            return IPAddress.GetLocalIPAddress();
        }

        public static int GetModuleId()
        {
            int module = 0;
            try
            {
                module = HttpContext.Current.Session["module_id"].ToInteger();
            }
            catch
            {
                module = 0;
            }
            return module;
        }
    }
}