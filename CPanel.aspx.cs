using DevExpress.Web;
using Newtonsoft.Json;
using CMS.Helper;
using CMS.Management.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace CMS
{
    public partial class CPanel : System.Web.UI.Page
    {
        public static string ErrorText = "";
        public static Exception exception = null;
        public static string url = getUrl();
        
        //Configuration for use Captcha or not
        public static bool UseCaptcha = false;
        //Configuration for use Logging or not
        public static bool Logging = false;
        //Configuration for use Logging or not
        public static bool AllowDuplicateLogin = false;

        private static string getUrl()
        {
            var url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.DnsSafeHost;
            string fullUrl = url + (HttpContext.Current.Request.ApplicationPath == "/"
                        ? string.Empty : HttpContext.Current.Request.ApplicationPath);
            return fullUrl;
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            UseCaptcha = ConfigurationManager.AppSettings.Get("Captcha").ToString().ToBoolean();
            Logging = ConfigurationManager.AppSettings.Get("Logging").ToString().ToBoolean();
            AllowDuplicateLogin = ConfigurationManager.AppSettings.Get("AllowDuplicateLogin").ToString().ToBoolean();            

            string RawUrl = Request.RawUrl;
            string AppPath = (Request.ApplicationPath == "/" ? string.Empty : HttpContext.Current.Request.ApplicationPath).ToString();
            string returnurl = "";

            if (AppPath != "" || AppPath != string.Empty)
            {
                RawUrl = RawUrl.Replace(AppPath, string.Empty);
            }
            returnurl = RawUrl;

            if (!Request.IsAuthenticated)
            {
                M_User.OwinSessionExpired();
                if (!IsPostBack && !IsCallback)
                {
                    Response.Redirect("~/account/login?returnurl=" + returnurl, false);
                }
                else
                {
                    string login = url + "/account/login?=" + returnurl;
                    Response.RedirectLocation = login;
                    Response.End();
                }
            }

            try
            {
                if (User.Identity.IsAuthenticated) 
                    this.Page.MasterPageFile = "~/Themes/" + User.Identity.Get_Theme() + "/" + User.Identity.Get_Theme() + ".Master";
                else
                    this.Page.MasterPageFile = "~/Site.master";

                if (User.Identity.IsLocked())
                    Response.Redirect("~/account/lock?returnurl=" + returnurl, false);

            }
            catch (Exception ex)
            {
                ex.Message.ToString();

                //Logout if theme doesnt exist
                Response.Redirect("~/account/logout");
            }

            //LoadUserSetting();

            string pageName = "";
            string abs_path = HttpContext.Current.Request.Url.AbsolutePath;
            string path = HttpContext.Current.Request.ApplicationPath;

            try
            {
                abs_path = (path != "/" ? abs_path.Replace(path, null) : abs_path).ToLower();
                DataTable page = Database.getDataTable(@"
                        SELECT M.module_title, M.module_id FROM m_module M 
                        WHERE LOWER(M.module_url)=LOWER(:module_url)",
                        new OracleParameter(":module_url", abs_path)
                    );

                if (page.Rows.Count > 0)
                {
                    pageName = ((page.Rows[0]["module_title"] == null || page.Rows[0]["module_title"].ToString() == string.Empty) || page.Rows[0]["module_title"].ToString() == ""
                    ? Path.GetFileName(HttpContext.Current.Request.PhysicalPath).Replace(".aspx", "").Replace("_", " ").ToTitleCase()
                    : page.Rows[0]["module_title"].ToString().ToTitleCase());

                    //Create module_id at runtime
                    HttpContext.Current.Session["module_id"] = page.Rows[0]["module_id"].ToInteger();
                }
                else
                {
                    pageName = Path.GetFileName(HttpContext.Current.Request.PhysicalPath).Replace(".aspx", "").Replace("_", " ").ToTitleCase();
                }
                page.Dispose();
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
                pageName = Path.GetFileName(HttpContext.Current.Request.PhysicalPath).Replace(".aspx", "").ToTitleCase();
            }

            Page.Title = "";
            Page.Title = (Page.Title == "" ? pageName.ToTitleCase() : Page.Title.ToTitleCase());
            Page.Title = ((Page.Title.ToLower() == "default" || Page.Title.ToLower() == "cpanel") ? "Card Management Dashboard" : Page.Title.ToTitleCase());

            NeedAccessRight access = (NeedAccessRight)Attribute.GetCustomAttribute(this.GetType(), typeof(NeedAccessRight));

            //GetRoleAction();

            //Send duplicate login
            if(!AllowDuplicateLogin) M_User.NotifyLogin(M_User.getLocalIP(), M_User.getUsername());
        }

        private void Page_Error(object sender, EventArgs e)
        {
            exception = Server.GetLastError();
            ErrorText = exception.Message;

            // Handle specific exception.
            if (exception is HttpUnhandledException)
            {
                ErrorText = "<b>HttpUnhandledException</b>. An error occurred on this page. Please verify your information to resolve the issue.";
            }

            // Clear the error from the server.
            //Server.ClearError();
        }

        public static void Search_TCode(object source, CallbackEventArgs e)
        {
            ASPxCallback aSPxCallback = (ASPxCallback)source;
            string tcode = e.Parameter.Trim();
            string url = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + (HttpContext.Current.Request.ApplicationPath == "/" ? string.Empty : HttpContext.Current.Request.ApplicationPath);

            switch (tcode.ToLower())
            {
                case "dashboard":                    
                    e.Result = "<script>window.location.replace(\"" + url + "\")</script>";
                    break;

                case "cpanel":
                    e.Result = "<script>window.location.replace(\"" + url + "\")</script>";
                    break;

                default:
                    DataTable dt = HttpContext.Current.Session["M_Module"] as DataTable;
                    DataTable dt_filter = new DataTable();
                    try
                    {
                        dt_filter = dt.Select(@"type_code ='" + tcode + "' OR module_name LIKE '%" + tcode + "%' AND module_url<>''").CopyToDataTable();
                    }
                    catch { }

                    dt_filter.Columns.Add("url", typeof(string));
                    dt_filter.Columns.Add("path", typeof(string));
                    if (dt_filter.Rows.Count > 0)
                    {
                        foreach(DataRow row in dt_filter.Rows)
                        {
                            row["url"] = url + row["module_url"].ToString();
                            row["module_icon"] = string.IsNullOrEmpty(row["module_icon"].ToString().Trim()) ? "fa-hashtag" : row["module_icon"].ToString();
                            row["path"] = row["module_url"].ToString();

                            List<string> split = row["path"].ToString().Split('/').ToList();
                            if (split.Any())
                            {
                                if (split.Count() > 0)
                                {
                                    split.RemoveAll(string.IsNullOrEmpty);
                                    row["path"] = string.Empty;
                                    Regex pattern = new Regex(@"[\/_-]");
                                    for (int x = 0; x <= split.Count() - 1; x++)
                                    {
                                        string uri = pattern.Replace(split[x], " ").Trim();
                                        string _title = ((uri == uri.ToUpper()) ? uri : uri.ToTitleCase());
                                        row["path"] += (x != split.Count() - 1) ? _title + "&nbsp;&lt;i class=&quot;fa fa-angle-right&quot;&gt;&lt;/i&gt;&nbsp;" : _title;
                                    }
                                }
                            }
                        }

                        if(dt_filter.Rows.Count == 1)
                        {
                            e.Result = "<script>window.location.replace(\"" + dt_filter.Rows[0]["url"].ToString() + "\")</script>";
                        }
                        else
                        {
                            string json = JsonConvert.SerializeObject(dt_filter);
                            string result = "<script>";
                            result += "var data=JSON.parse('"+ json +"');";
                            result += "$('ul.search-result').html(null);";
                            result += "data.forEach(function(itm, idx){";
                            result += "$('ul.search-result').append('";
                            result += "<li class=\"mdc-list-item px-3\" role=\"menuitem\" tabindex=\"-1\" onclick=\"javascript:openLink(this)\">";
                            result += "<div class=\"item-thumbnail item-thumbnail-icon-only mr-3\"><i class=\"fa fa-1x '+ itm.module_icon +'\"></i></div>";
                            result += "<div class=\"item-content d-flex align-items-start flex-column justify-content-center\">";
                            result += "<h6 class=\"item-subject font-weight-bold mt-1 mb-0\"><a class=\"mdc-typography mdc-theme--dark\" href=\"'+ itm.url +'\" tabindex=\"-1\">'+ itm.module_name +'</a></h6>";
                            result += "<small class=\"text-muted\">'+ $(\"<textarea/>\").html(itm.path).text() +'</small>";
                            result += "</div>";
                            result += "</li>";
                            result += "');";
                            result += "});";
                            result += "$('button.search-button').trigger('click');";
                            result += "console.log(data);";
                            result += "</script>";
                            e.Result = result;
                        }
                    }
                    break;
            }        
        }

        public void AddError(Dictionary<GridViewColumn, string> errors, GridViewColumn column, string errorText)
        {
            if (errors.ContainsKey(column)) return;
            errors[column] = errorText;
        }

    }
}