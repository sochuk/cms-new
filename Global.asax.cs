using API;
using CMS.Content.Themes;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CMS
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);            
            Material.RegisterBundles(BundleTable.Bundles);
            //CoreUI.RegisterBundles(BundleTable.Bundles);

            //Web API Configuration
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            DevExpress.Web.ASPxWebControl.CallbackError += Callback_Error;

            //new Notification.WhatsApp();
        }

        protected void Callback_Error(object sender, EventArgs e)
        {
            var exception = HttpContext.Current.Server.GetLastError();
            //Check exception type and specify error text for your exception
            if (exception is Exception)
            {
                DevExpress.Web.ASPxWebControl.SetCallbackErrorMessage(exception.Message);
            }
        }

        protected void Application_End()
        {
            //Notification.WhatsApp.Dispose();
        }
    }
}