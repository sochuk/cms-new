using DevExpress.Web;
using CMS.Helper;
using CMS.Management.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace CMS.Themes.Material
{
    public partial class MaterialSite : SiteMaster
    {
        public DataTable menu_root = new DataTable();
        public DataTable menu_sub = new DataTable();
        public DataTable notification = new DataTable();
        public DataTable activity = new DataTable();
        public DataTable online = new DataTable();
        public static ASPxCallback ASPxPanel;
        public static string Navigation_Breadcrumb;

        public int badge_notification;        
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!IsPostBack)
            {
                // Menu
                if (HttpContext.Current.User.IsInRole("Super Administrator"))
                {
                    HttpContext current;
                    if (HttpContext.Current.Session == null)
                    {
                        current = HttpContext.Current;
                    }                    

                    if (HttpContext.Current.Session["M_Module_Root"] == null || HttpContext.Current.Session["M_Module"] == null)
                    {
                        menu_root = Database.getDataTable(@"
                        SELECT M.type_code, M.module_id, M.module_name, M.module_root, M.module_icon, M.module_url, M.module_type
                        FROM m_module  M
                        WHERE M.module_root=0 AND M.isdelete=0 AND M.isactive=1 AND M.isvisible=1
                        ORDER BY M.order_no, M.module_name");

                        menu_sub = Database.getDataTable(@"
                        SELECT M.type_code, M.module_id, M.module_name, M.module_root, M.module_icon, M.module_url, M.module_type
                        FROM m_module  M
                        WHERE M.module_root <> 0 AND M.isdelete=0 AND M.isactive=1 AND M.isvisible=1 ORDER BY M.order_no, M.module_name");

                        HttpContext.Current.Session["M_Module_Root"] = menu_root;
                        HttpContext.Current.Session["M_Module"] = menu_sub;
                    }
                    else
                    {
                        menu_root = HttpContext.Current.Session["M_Module_Root"] as DataTable;
                        menu_sub = HttpContext.Current.Session["M_Module"] as DataTable;
                    }
                }
                else
                {
                    if (HttpContext.Current.Session["M_Module_Root"] == null || HttpContext.Current.Session["M_Module"] == null)
                    {
                        menu_root = Database.getDataTable(@"SELECT M.type_code, M.module_id, M.module_name, M.module_root, M.module_icon, M.module_url, M.module_type 
                            FROM m_access  A
                            LEFT JOIN m_module  M ON A.module_id=M.module_id
                            WHERE A.group_id=:group_id AND M.module_root=0 AND M.isdelete=0 AND M.isactive=1 ORDER BY M.order_no, M.module_name",
                            new OracleParameter(":group_id", HttpContext.Current.User.Identity.Get_GroupID()));

                        menu_sub = Database.getDataTable(@"SELECT M.type_code, M.module_id, M.module_name, M.module_root, M.module_icon, M.module_url, M.module_type 
                            FROM m_access  A
                            LEFT JOIN m_module  M ON A.module_id=M.module_id
                            WHERE A.group_id=:group_id AND M.module_root <> 0 AND M.isdelete=0 AND M.isactive=1 ORDER BY M.order_no, M.module_name",
                            new OracleParameter(":group_id", HttpContext.Current.User.Identity.Get_GroupID()));

                        HttpContext.Current.Session["M_Module_Root"] = menu_root;
                        HttpContext.Current.Session["M_Module"] = menu_sub;
                    }
                    else
                    {
                        menu_root = HttpContext.Current.Session["M_Module_Root"] as DataTable;
                        menu_sub = HttpContext.Current.Session["M_Module"] as DataTable;
                    }

                }


            }

            Navigation_Breadcrumb = getFriendlyUrl();
        }

        protected void TCode_Callback(object source, CallbackEventArgs e)
        {
            ASPxCallback aSPxCallback = (ASPxCallback)source;
            CPanel.Search_TCode(aSPxCallback, e);
        }

        public string getFriendlyUrl()
        {            
            Navigation_Breadcrumb = string.Empty;

            string abs_path = HttpContext.Current.Request.Url.AbsolutePath;
            string path = HttpContext.Current.Request.ApplicationPath;
            abs_path = (path != "/" ? abs_path.Replace(path, null) : abs_path);

            List<string> path_split = abs_path.Split('/').ToList();
            path_split.RemoveAll(String.IsNullOrEmpty);

            if (path_split.Any())
            {
                if (path_split.Count() > 0)
                {
                    Navigation_Breadcrumb = "<nav class=\"d-none d-md-block\" aria-label=\"breadcrumb\">";
                    Navigation_Breadcrumb += "<ol class=\"breadcrumb\">";
                    Navigation_Breadcrumb += "<li class=\"breadcrumb-item\"><a href=\""+ CPanel.url +"\">CPanel</a></li>";

                    Regex pattern = new Regex(@"[\/_-]");
                    string _url = CPanel.url + "/";
                    for (int x=0; x < path_split.Count(); x++)
                    {
                        if (x == path_split.Count() - 1)
                        {
                            string _title = Page.Title.ToTitleCase();
                            Navigation_Breadcrumb += "<li class=\"breadcrumb-item active\">" + _title + "</li>";
                        }
                        else
                        {
                            _url += path_split[x] + "/";
                            string uri = pattern.Replace(path_split[x], " ").Trim();
                            string _title = ((uri == uri.ToUpper()) ? uri : uri.ToTitleCase());
                            Navigation_Breadcrumb += "<li class=\"breadcrumb-item\"><a href=\"#\">" + _title + "</a></li>";
                        }
                    }

                    Navigation_Breadcrumb += "</ol>";
                    Navigation_Breadcrumb += "</nav>";
                }
            }
            return Navigation_Breadcrumb;
        }

        protected void GridHystory_Init(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;

            (grid.Columns["user_id"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_User.SelectAll();
            (grid.Columns["log_type"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = typeof(Log.LogType).ToDataTable("log_type", "log_type_desc");
            grid.DataSource = Log.SelectAll(Helper.Application.GetModuleId());
            grid.DataBind();

            //grid.ClearSort();
            //grid.GroupBy(grid.Columns["log_date"]);
        }

        protected void grid_detail_BeforePerformDataSelect(object sender, EventArgs e)
        {
            ASPxGridView grid_detail = sender as ASPxGridView;
            var key = grid_detail.GetMasterRowKeyValue().ToInteger();
            grid_detail.DataSource = Log.GetDetail<M_User>(key);
        }
    }
}