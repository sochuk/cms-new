using System.Web.Optimization;

namespace CMS.Content.Themes
{
    public class Material
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/themes/material/css/iconpack").Include(
                            "~/Themes/material/css/icons/material-designicons.css",
                            "~/Themes/Material/css/icons/flag-icon.css",
                            "~/Themes/Material/css/icons/fontawesome.css",
                            "~/Themes/Material/css/icons/simple-line-icons.css",
                            "~/Themes/Material/css/icons/coreui-icons.css"
                            ));

            bundles.Add(new StyleBundle("~/themes/material/css/light").Include(                            
                            "~/Themes/material/css/vendor.bundle.base.css",
                            "~/Themes/material/css/material-bootstrap.css",                            
                            "~/Themes/material/css/pace.min.css",
                            "~/Themes/material/css/style2.css",                           
                            "~/Content/css/dx.css",
                            "~/Themes/material/css/components/bootstrapselect.min.css",
                            "~/Themes/material/css/components/datetimepicker.min.css",
                            "~/Themes/material/css/components/fullcalendar.min.css",
                            "~/Themes/material/css/components/jquery.toast.css",
                            "~/Themes/material/css/components/jquery.notify.css",
                            "~/Themes/material/css/components/sweetalert.min.css",
                            "~/Themes/material/css/components/jquery.bar.rating.css",
                            "~/Themes/material/css/components/jquery-jvectormap.css"
                            ));

            bundles.Add(new StyleBundle("~/themes/material/css/dark").Include(
                            "~/Themes/material/css/vendor.bundle.base.css",
                            "~/Themes/material/css/material-bootstrap.css",
                            "~/Themes/material/css/pace.min.css",
                            "~/Themes/material/css/dark/style.css",
                            "~/Content/css/dx.css",
                            "~/Themes/material/css/components/bootstrapselect.min.css",
                            "~/Themes/material/css/components/datetimepicker.min.css",
                            "~/Themes/material/css/components/fullcalendar.min.css",
                            "~/Themes/material/css/components/jquery.toast.css",
                            "~/Themes/material/css/components/jquery.notify.css",
                            "~/Themes/material/css/components/sweetalert.min.css",
                            "~/Themes/material/css/components/jquery.bar.rating.css",
                            "~/Themes/material/css/components/jquery-jvectormap.css"
                            ));

            bundles.Add(new StyleBundle("~/themes/material/css/login").Include(
                            "~/Themes/Material/css/vendor.bundle.base.css",
                            "~/Themes/Material/css/material-bootstrap.css",
                            "~/Themes/Material/css/style.css",
                            "~/Content/css/dx.css"
                            ));

            bundles.Add(new ScriptBundle("~/themes/material/js/preloader").Include(
                            "~/Themes/material/js/preloader.js"
                            ));

            bundles.Add(new ScriptBundle("~/themes/material/js/vendorbundle").Include(
                            "~/Themes/material/js/vendor.bundle.base.js"
                            ));

            bundles.Add(new ScriptBundle("~/themes/material/js/data").Include(
                            "~/Themes/material/js/popper.min.js",
                            "~/Themes/material/js/material.js",
                            "~/Themes/material/js/material-bootstrap.min.js",
                            "~/Themes/material/js/pace.min.js",
                            "~/Content/js/jscookies.js",
                            "~/Content/js/dx.js",
                            "~/Themes/material/js/jquery.validate.min.js",
                            "~/Themes/material/js/jquery.validation.additional-methods.js",
                            "~/Themes/material/js/bootstrap-maxlength.min.js",
                            "~/Themes/material/js/jquery.toast.min.js",
                            "~/Themes/material/js/jquery.notify.min.js",
                            "~/Themes/material/js/misc.js",
                            "~/Themes/Material/js/sweetalert.min.js",
                            "~/Themes/Material/js/avatar.text.js",
                            "~/Themes/Material/js/jquery.pretty.date.js",
                            "~/Themes/Material/js/jquery-jvectormap.min.js",
                            "~/Themes/Material/js/jquery-jvectormap-world-mill-en.js"
                            ));

            bundles.Add(new ScriptBundle("~/themes/material/js/login").Include(
                            "~/Themes/material/js/popper.min.js",
                            "~/Themes/material/js/material.js",
                            "~/Themes/material/js/material-bootstrap.min.js",                            
                            "~/Themes/material/js/jquery.toast.min.js",
                            "~/Themes/material/js/jquery.validate.min.js",
                            "~/Themes/material/js/jquery.validation.additional-methods.js",
                            "~/Themes/material/js/jquery.countdown.min.js",
                            "~/Themes/material/js/login.js",
                            "~/Themes/material/js/avatar.text.js",
                            "~/Themes/Material/js/sweetalert.min.js",
                            "~/Content/js/dx.js"
                            ));

            bundles.Add(new ScriptBundle("~/themes/material/js/signalr").Include(
                            "~/Content/js/jquery.signalR-2.4.1.min.js",
                            "~/Themes/material/js/hub.js"
                            ));

            bundles.Add(new ScriptBundle("~/themes/material/js/dxfunction").Include(
                            "~/Content/js/dx.function.js"
                            ));

            bundles.Add(new ScriptBundle("~/themes/material/js/jquery").Include(
                            "~/Content/js/jquery-3.4.1.min.js"
                            ));

            
        }
    }
}