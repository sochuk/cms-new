using DevExpress.Web;
using Newtonsoft.Json.Linq;
using CMS.Helper;
using CMS.Management.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace CMS.Management
{
    public partial class Group : CPanel
    {
        public DataTable dt = new DataTable();
        public DataTable role = new DataTable();

        public DataTable dt_temp = new DataTable();
        string[] column_name = new string[] { "Group Name", "Description" };
        string[] row_field = new string[] { "group_name", "group_desc" };

        [PrincipalPermission(SecurityAction.Demand, Role = "Super Administrator")]
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();

            grid.bindDelete(grid_delete, dt_temp, column_name, row_field);
        }

        public void LoadData()
        {
            dt = M_Group.SelectAll();
            dt.Columns.Add("allow_edit", typeof(bool));
            foreach (DataRow row in dt.Rows)
            {
                if (row["role_name"].ToString() == "Super Administrator")
                {
                    row["allow_edit"] = false;
                }
                else
                {
                    row["allow_edit"] = true;
                }
            }

            (grid.Columns["role_id"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_Role.SelectAll();
            (grid.Columns["createby"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_User.BindData();
            (grid.Columns["updateby"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_User.BindData();

            grid.DataSource = dt;
            grid.DataBind();

            grid.SettingsExport.FileName = Page.Title + " (" + DateTime.Now.ToString("yyyyMMddHHmmss") + ")";
        }

        protected void grid_Init(object sender, EventArgs e)
        {
            grid = (ASPxGridView)sender;
            grid.initializeGrid();
        }

        protected void grid_CustomButtonInitialize(object sender, DevExpress.Web.ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;
            grid = (ASPxGridView)sender;
            grid.initializeCustomButton(e);
            if (e.ButtonID == "ButtonEnable" || e.ButtonID == "ButtonDisable")
            {
                if(e.VisibleIndex >= 0)
                {
                    string role = grid.GetRowValues(e.VisibleIndex, "role_name").ToString();
                    if (role == "Super Administrator") e.Visible = DevExpress.Utils.DefaultBoolean.False;
                }                
            }
        }

        protected void grid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;
            grid = (ASPxGridView)sender;
            if(e.ButtonType == ColumnCommandButtonType.SelectCheckbox)
            {
                string role = grid.GetRowValues(e.VisibleIndex, "role_name").ToString();
                if (role == "Super Administrator") e.Visible = false;
            }

            if (e.ButtonType == ColumnCommandButtonType.Edit || e.ButtonType == ColumnCommandButtonType.Delete)
            {
                string role = grid.GetRowValues(e.VisibleIndex, "role_name").ToString();
                if (role == "Super Administrator")
                {
                    e.Visible = false;
                }
            }
        }        

        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            grid = (ASPxGridView)sender;
            grid.initializeHtmlRowPrepared(e);
        }

        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            grid = (ASPxGridView)sender;
            grid.initializeCustomCallback(e);
            List<object> selected = grid.GetSelectedKey("group_id");
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
                                M_Group.Delete(new M_Group { group_id = id.ToInteger() }, cnn, sqlTransaction);
                            }
                            Log.Insert(Log.LogType.DELETE, "Delete group", JObject.FromObject(new { group_id = selected }), cnn, sqlTransaction);
                            sqlTransaction.Commit();
                        }
                        grid.JSProperties["cpShowDeleteConfirm"] = false;
                        grid.JSProperties["cpRefresh"] = true;
                        grid.alertSuccess("Data deleted successfully");
                    }
                    break;
            }
        }

        protected void grid_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            grid = (ASPxGridView)sender;
            grid.initializeCustomButtonCallback(e);

            List<object> selected = grid.GetSelectedKey("group_id");

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
                                M_Group.Enable(new M_Group { group_id = item.ToString().ToInteger() }, cnn, sqlTransaction);
                            }
                            Log.Insert(Log.LogType.ENABLE, "Enable group", JObject.FromObject(new { group_id = selected }), cnn, sqlTransaction);
                            sqlTransaction.Commit();
                            grid.Refresh();
                            break;
                        case "BUTTONDISABLE":
                            foreach (var item in selected)
                            {
                                M_Group.Disable(new M_Group { group_id = item.ToString().ToInteger() }, cnn, sqlTransaction);
                            }
                            Log.Insert(Log.LogType.DISABLE, "Disable group", JObject.FromObject(new { group_id = selected }), cnn, sqlTransaction);
                            sqlTransaction.Commit();
                            grid.Refresh();
                            break;
                    }

                }
            }
        }

        protected void grid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            grid = (ASPxGridView)sender;
            if (grid.IsNewRowEditing)
            {
                if (M_Group.IsExist(new M_Group { group_name = e.NewValues["group_name"].ToString() }))
                {
                    AddError(e.Errors, grid.Columns["group_name"], "* Data exist");
                }
            }
            else
            {
                if (e.NewValues["group_name"].ToString().Trim() != e.OldValues["group_name"].ToString().Trim())
                {
                    if (M_Group.IsExist(new M_Group { group_name = e.NewValues["group_name"].ToString() }))
                    {
                        AddError(e.Errors, grid.Columns["group_name"], "* Data exist");
                    }
                }
            }
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            (sender as ASPxGridView).DataSource = dt;
        }

        protected void grid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            grid = (ASPxGridView)sender;
            using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
            {
                cnn.Open();
                using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                {
                    ASPxGridLookup role_id = grid.FindID("role_id", "lookup_role");

                    M_Group group = new M_Group();
                    group.group_name = (string)e.NewValues["group_name"];
                    group.group_desc = (string)e.NewValues["group_desc"];
                    group.role_id = role_id.Value == null ? 0 : (int)role_id.Value;

                    try
                    {
                        group = M_Group.Insert(group, cnn, sqlTransaction);
                        e.NewValues.Add(nameof(group.group_id), group.group_id);
                        Log.Insert(Log.LogType.ADD, "Add group", JObject.FromObject(e.NewValues), cnn, sqlTransaction);
                        sqlTransaction.Commit();
                        grid.alertSuccess();
                    }
                    catch (Exception ex)
                    {
                        ex.Message.ToString();
                        grid.alertError(ex.Message);
                        sqlTransaction.Rollback();
                        throw new Exception(ex.Message);
                    }

                    (sender as ASPxGridView).CancelEdit();
                    LoadData();
                }
                cnn.Close();
            }
            e.Cancel = true;
        }

        protected void grid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            grid = (ASPxGridView)sender;
            using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
            {
                cnn.Open();
                using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                {
                    ASPxGridLookup role_id = grid.FindID("role_id", "lookup_role");

                    M_Group group = new M_Group();
                    group.group_id = (int)e.Keys["group_id"];
                    group.group_name = (string)e.NewValues["group_name"];
                    group.group_desc = (string)e.NewValues["group_desc"];
                    group.role_id = role_id == null ? 0 : (int)role_id.Value;

                    try
                    {
                        M_Group.Update(group, cnn, sqlTransaction);
                        e.NewValues.Add(nameof(group.group_id), group.group_id);
                        Log.Insert(Log.LogType.UPDATE, "Update group", JObject.FromObject(e.NewValues), cnn, sqlTransaction);
                        sqlTransaction.Commit();
                        grid.alertSuccess();
                    }
                    catch (Exception ex)
                    {
                        ex.Message.ToString();
                        grid.alertError(ex.Message.ToString());
                        sqlTransaction.Rollback();
                        throw new Exception(ex.Message);
                    }

                    (sender as ASPxGridView).CancelEdit();
                    LoadData();
                }
                cnn.Close();
            }
            e.Cancel = true;
        }

        protected void Role_Id_Init(object sender, EventArgs e)
        {
            ASPxGridLookup lookup = sender as ASPxGridLookup;
            lookup.DataSource = M_Role.SelectAll();
            lookup.Bind("role_id");
        }

        protected void grid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            grid = sender as ASPxGridView;
            grid.ShowDeleteConfirm(e);
        }
    }
}