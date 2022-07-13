using DevExpress.Web;
using System;
using CMS.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Management.Model;
using CMS.Context;

namespace CMS.Logs
{
    [NeedAccessRight]
    public partial class UserLog : CPanel
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            (grid.Columns["LOG_TYPE"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = typeof(Log.LogType).ToDataTable("LOG_TYPE", "LOG_TYPE_DESC");
            (grid.Columns["USER_ID"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_User.BindData();
            (grid.Columns["MODULE_ID"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_Module.SelectAll();

            //grid.DataSource = Log.SelectAll();
            //grid.DataBind();

            grid.SettingsExport.FileName = Page.Title + " (" + DateTime.Now.ToString("yyyyMMddHHmmss") + ")";

            grid.GroupBy(grid.Columns["MODULE_ID"]);
        }

        protected void grid_Init(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            grid.initializeGrid(false);
        }

        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            grid.initializeHtmlRowPrepared(e);
        }

        protected void grid_detail_BeforePerformDataSelect(object sender, EventArgs e)
        {
            ASPxGridView grid_detail = sender as ASPxGridView;
            var key = grid_detail.GetMasterRowKeyValue().ToInteger();
            grid_detail.DataSource = Log.GetDetail<M_User>(key);
        }

        protected void grid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            if (e.Column.FieldName != "MODULE_ID") return;
            if (e.Value.ToInteger() == 0) e.DisplayText = "Other";
        }

        protected void UserLogData_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "LOG_ID";
            var db = new CMSContext();
            var data = from a in db.LOG_USER
                       select a;
            e.QueryableSource = data;
        }
    }
}