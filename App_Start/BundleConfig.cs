using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace CMS
{
    public class BundleConfig
    {
        // For more information on Bundling, visit https://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                            "~/Content/js/WebForms/WebForms.js",
                            "~/Content/js/WebForms/WebUIValidation.js",
                            "~/Content/js/WebForms/MenuStandards.js",
                            "~/Content/js/WebForms/Focus.js",
                            "~/Content/js/WebForms/GridView.js",
                            "~/Content/js/WebForms/DetailsView.js",
                            "~/Content/js/WebForms/TreeView.js",
                            "~/Content/js/WebForms/WebParts.js"));

            // Order is very important for these files to work, they have explicit dependencies
            bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
                    "~/Content/js/WebForms/MsAjax/MicrosoftAjax.js",
                    "~/Content/js/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                    "~/Content/js/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                    "~/Content/js/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));

            // Use the Development version of Modernizr to develop with and learn from. Then, when you’re
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                            "~/Content/js/modernizr-*"));

            ScriptManager.ScriptResourceMapping.GetDefinition("MicrosoftAjaxHistory.js").Path = "~/Content/js/WebForms/MsAjax/MicrosoftAjaxHistory.js";
            ScriptManager.ScriptResourceMapping.GetDefinition("MicrosoftAjaxCore.js").Path = "~/Content/js/WebForms/MsAjax/MicrosoftAjaxCore.js";
            ScriptManager.ScriptResourceMapping.GetDefinition("MicrosoftAjaxComponentModel.js").Path = "~/Content/js/WebForms/MsAjax/MicrosoftAjaxComponentModel.js";
            ScriptManager.ScriptResourceMapping.GetDefinition("MicrosoftAjaxNetwork.js").Path = "~/Content/js/WebForms/MsAjax/MicrosoftAjaxNetwork.js";
            ScriptManager.ScriptResourceMapping.GetDefinition("MicrosoftAjaxApplicationServices.js").Path = "~/Content/js/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js";
            ScriptManager.ScriptResourceMapping.GetDefinition("MicrosoftAjaxGlobalization.js").Path = "~/Content/js/WebForms/MsAjax/MicrosoftAjaxGlobalization.js";
            ScriptManager.ScriptResourceMapping.GetDefinition("MicrosoftAjaxWebForms.js").Path = "~/Content/js/WebForms/MsAjax/MicrosoftAjaxWebForms.js";
            ScriptManager.ScriptResourceMapping.GetDefinition("MicrosoftAjax.js").Path = "~/Content/js/WebForms/MsAjax/MicrosoftAjax.js";
            ScriptManager.ScriptResourceMapping.GetDefinition("MicrosoftAjaxWebServices.js").Path = "~/Content/js/WebForms/MsAjax/MicrosoftAjaxWebServices.js";
            ScriptManager.ScriptResourceMapping.GetDefinition("MicrosoftAjaxTimer.js").Path = "~/Content/js/WebForms/MsAjax/MicrosoftAjaxTimer.js";
            ScriptManager.ScriptResourceMapping.GetDefinition("MicrosoftAjaxSerialization.js").Path = "~/Content/js/WebForms/MsAjax/MicrosoftAjaxSerialization.js";

            var jquery = ScriptManager.ScriptResourceMapping.GetDefinition("jquery");
            if(jquery != null)
            {
                jquery.Path = "~/Content/js/jquery-3.5.1.min.js";
                jquery.DebugPath = "~/Content/js/jquery-3.5.1.js";
            }
            else
            {
                ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
                {
                    Path = "~/Content/js/jquery-3.5.1.min.js",
                    DebugPath = "~/Content/js/jquery-3.5.1.js",
                    LoadSuccessExpression = "jquery"
                });
            }
            

            var bootstrap = ScriptManager.ScriptResourceMapping.GetDefinition("bootstrap");
            if (bootstrap != null)
            {
                bootstrap.Path = "~/Content/js/bootstrap.min.js";
                bootstrap.DebugPath = "~/Content/js/bootstrap.js";
            }
            else
            {
                ScriptManager.ScriptResourceMapping.AddDefinition("bootstrap", new ScriptResourceDefinition
                {
                    Path = "~/Content/js/bootstrap.min.js",
                    DebugPath = "~/Content/js/bootstrap.js",
                    LoadSuccessExpression = "bootstrap"
                });
            }
                

            ScriptManager.ScriptResourceMapping.AddDefinition("popper", new ScriptResourceDefinition() {
                Path = "~/Content/js/umd/popper.min.js",
                DebugPath = "~/Content/js/umd/popper.js"
            });

            bundles.Add(new ScriptBundle("~/bundles/js").Include(                            
                            "~/Content/js/notify.js"
                            ));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                            "~/Content/css/notify.min.css"
                            ));

            bundles.Add(new ScriptBundle("~/Content/js/dev/devextreme").Include(
                            "~/Content/js/dev/cldr.min.js",
                            "~/Content/js/dev/cldr/event.min.js",
                            "~/Content/js/dev/cldr/supplemental.min.js",
                            "~/Content/js/dev/globalize.min.js",
                            "~/Content/js/dev/globalize/message.min.js",
                            "~/Content/js/dev/globalize/number.min.js",
                            "~/Content/js/dev/globalize/currency.min.js",
                            "~/Content/js/dev/globalize/date.min.js",
                            "~/Content/js/dev/dx.all.js",
                            "~/Content/js/dev/knockout-latest.js"
                            ));

            bundles.Add(new StyleBundle("~/Content/css/dev/devextreme").Include(
                            "~/Content/css/dev/dx.light.css",
                            "~/Content/css/dev/dx.common.css"
                            ));
        }
    }
}