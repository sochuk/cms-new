using DevExpress.Web;
using DevExpress.Web.ASPxDiagram;
using DevExpress.Web.ASPxTreeList;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KMS.Helper;
using KMS.Management.Model;
using KMS.Management.Structure.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;

namespace KMS.Management.Structure
{
    public partial class Organization : CPanel
    {
        public DataTable dt = new DataTable();
        public DataTable dt_temp = new DataTable();

        string[] column_name = new string[] { "Department Name", "Division Name", "User" };
        string[] row_field = new string[] { "department_name", "division_name", "fullname" };

        protected void Page_Load(object sender, EventArgs e)
        {                  
            LoadData();
        }

        public void LoadData()
        {
            (grid.Columns["division_id"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_Division.SelectAll();
            (grid.Columns["user_id"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_User.BindData();
            (grid.Columns["user_root"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_User.BindData();
            (grid.Columns["createby"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_User.BindData();
            (grid.Columns["updateby"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_User.BindData();

            DataTable data = M_Organization.SelectAll();
            grid.DataSource = data;
            grid.DataBind();

            treelist.DataSource = grid.DataSource;
            treelist.DataBind();
            treelist.ExpandToLevel(1);

            if (!IsPostBack)
            {
                diagram.NodeDataSource = grid.DataSource;
                diagram.DataBind();
            }

            //Setting filename export data
            treelist.SettingsExport.FileName = Page.Title + DateTime.Now.ToString("yyyyMMddHHmmss");
            //Setting filename export data
            grid.SettingsExport.FileName = Page.Title + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        #region "Grid"
        protected void grid_Init(object sender, EventArgs e)
        {
            grid = (ASPxGridView)sender;
            grid.initializeGrid();
        }

        protected void grid_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            grid = (ASPxGridView)sender;
            grid.initializeHtmlRowPrepared(e);
        }

        protected void grid_CustomButtonInitialize(object sender, DevExpress.Web.ASPxGridViewCustomButtonEventArgs e)
        {
            grid = (ASPxGridView)sender;
            grid.initializeCustomButton(e);
        }

        protected void grid_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            grid = (ASPxGridView)sender;
            grid.initializeCustomButtonCallback(e);
            List<object> selected = grid.GetSelectedKey("organization_id");
            using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
            {
                cnn.Open();
                using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                {
                    switch (e.ButtonID.ToUpper())
                    {
                        case "BUTTONENABLE":
                            foreach (var item in selected)
                            {
                                M_Organization.Enable(new M_Organization { organization_id = item.ToString().ToInteger() }, cnn, sqlTransaction);
                            }
                            Log.Insert(Log.LogType.ENABLE, "Enable organization", JObject.FromObject(new { organization_id = selected }), cnn, sqlTransaction);
                            sqlTransaction.Commit();
                            grid.JSProperties["cpRefresh"] = true;
                            break;
                        case "BUTTONDISABLE":
                            foreach (var item in selected)
                            {
                                M_Organization.Disable(new M_Organization { organization_id = item.ToString().ToInteger() }, cnn, sqlTransaction);
                            }
                            Log.Insert(Log.LogType.DISABLE, "Disable organization", JObject.FromObject(new { organization_id = selected }), cnn, sqlTransaction);
                            sqlTransaction.Commit();
                            grid.JSProperties["cpRefresh"] = true;
                            break;
                    }

                }
            }
        }

        protected void grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            grid = (ASPxGridView)sender;
            grid.initializeCustomCallback(e);

            List<object> selected = grid.GetSelectedKey("organization_id");

            switch (e.Parameters.ToUpper())
            {
                case "DELETE":

                    using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
                    {
                        cnn.Open();
                        using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                        {
                            foreach (var item in selected)
                            {
                                var id = item.ToString();
                                M_Organization.Delete(new M_Organization { organization_id = id.ToInteger() }, cnn, sqlTransaction);
                            }
                            Log.Insert(Log.LogType.DELETE, "Delete division", JObject.FromObject(new { organization_id = selected }), cnn, sqlTransaction);
                            sqlTransaction.Commit();
                        }
                        grid.Refresh();
                    }
                    break;
            }
        }

        protected void grid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            grid = (ASPxGridView)sender;
            using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
            {
                cnn.Open();
                using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                {                    
                    ASPxGridLookup user_root = grid.FindID("user_root", "lookup_user_root");
                    ASPxGridLookup division = grid.FindID("division_id", "lookup_division");                   

                    try
                    {
                        M_Organization data = new M_Organization();
                        data.division_id = division.Value == null ? 0 : (int)division.Value;
                        data.user_id = (int)e.OldValues["user_id"];
                        data.user_root = user_root.Value == null ? 0 : (int)user_root.Value;
                        data.can_approve = e.NewValues["can_approve"] == null ? false : (bool)e.NewValues["can_approve"];

                        data = M_Organization.InsertUpdate(data, cnn, sqlTransaction);
                        Log.Insert(Log.LogType.UPDATE, "Add/Update division", JObject.FromObject(e.NewValues), cnn, sqlTransaction);
                        sqlTransaction.Commit();
                        grid.alertSuccess();
                    }
                    catch (Exception ex)
                    {
                        grid.alertError(ex.Message.ToString());
                        sqlTransaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                    (sender as ASPxGridView).CancelEdit();
                    //Refresh gridView
                    LoadData();
                }
                cnn.Close();
            }
            e.Cancel = true;
        }

        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            if (e.Column.FieldName == "user_id") e.Editor.ReadOnly = !grid.IsNewRowEditing;
        }

        protected void lookup_division_Init(object sender, EventArgs e)
        {
            ASPxGridLookup lookup = sender as ASPxGridLookup;
            lookup.DataSource = M_Division.SelectAll();
            lookup.Bind("division_id");
        }

        protected void lookup_user_root_Init(object sender, EventArgs e)
        {
            ASPxGridLookup lookup = sender as ASPxGridLookup;
            lookup.DataSource = M_User.SelectAll();
            lookup.Bind("user_root");
        }
        #endregion

        #region "TreeList"
        protected void treelist_Init(object sender, EventArgs e)
        {
            treelist = (ASPxTreeList)sender;
            treelist.initializeTreeList(e);
        }

        protected void treelist_CustomCallback(object sender, TreeListCustomCallbackEventArgs e)
        {
            treelist = (ASPxTreeList)sender;
            treelist.initializeCustomCallback(e);         
        }

        protected void treelist_CommandColumnButtonInitialize(object sender, TreeListCommandColumnButtonEventArgs e)
        {
            treelist = (ASPxTreeList)sender;
            treelist.initializeCommandButton(e);
        }

        #endregion

        protected void cpDiagram_Callback(object sender, CallbackEventArgsBase e)
        {
            diagram.Import("");
            diagram.NodeDataSource = grid.DataSource;
            diagram.DataBind();
            string json = diagram.Export();
        }

        protected void grid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if(e.Column.FieldName == "user_root")
            {
                if (e.Value.ToString() == "0") e.DisplayText = "";
            }
        }
    }
}