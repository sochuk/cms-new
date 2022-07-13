using DevExpress.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CMS.Helper;
using CMS.Management.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace CMS.Management
{
    
    public partial class Role : CPanel
    {
        public DataTable dt = new DataTable();
        public DataTable dt_temp = new DataTable();

        string[] column_name = new string[] { "Role Name", "Description" };
        string[] row_field = new string[] { "role_name", "role_desc" };

        [PrincipalPermission(SecurityAction.Demand, Role = "Super Administrator")]
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();

            grid.bindDelete(grid_delete, dt_temp, column_name, row_field);

        }

        public void LoadData()
        {
            dt = M_Role.SelectAll();
            dt.Columns.Add("allow_edit", typeof(bool));
            foreach(DataRow row in dt.Rows)
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

            (grid.Columns["createby"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_User.BindData();
            (grid.Columns["updateby"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_User.BindData();

            grid.DataSource = dt;            
            grid.DataBind();

            //Setting filename export data
            grid.SettingsExport.FileName = Page.Title + DateTime.Now.ToString("yyyyMMddHHmmss");
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

            if (e.ButtonID == "ButtonEdit" || e.ButtonID == "ButtonDelete" || e.ButtonID == "ButtonEnable" || e.ButtonID == "ButtonDisable")
            {
                string role = grid.GetRowValues(e.VisibleIndex, "role_name").ToString();
                if (role == "Super Administrator") {
                    e.Enabled = false;
                    e.Visible = DevExpress.Utils.DefaultBoolean.False;                    
                }
            }
        }

        protected void grid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;
            grid = (ASPxGridView)sender;
            if (e.ButtonType == ColumnCommandButtonType.SelectCheckbox)
            {
                string role = grid.GetRowValues(e.VisibleIndex, "role_name").ToString();
                if (role == "Super Administrator") {
                    e.Visible = false;
                    e.Enabled = false;
                } 
            }

            if(e.ButtonType == ColumnCommandButtonType.Edit || e.ButtonType == ColumnCommandButtonType.Delete)
            {
                string role = grid.GetRowValues(e.VisibleIndex, "role_name").ToString();
                if (role == "Super Administrator")
                {
                    e.Visible = false;
                    e.Enabled = false;
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
            List<object> selected = grid.GetSelectedKey("role_id");
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
                                M_Role.Delete(new M_Role { role_id = id.ToInteger() }, cnn, sqlTransaction);                                
                            }
                            Log.Insert(Log.LogType.DELETE, "Delete role", JObject.FromObject(new { role_id = selected }), cnn, sqlTransaction);
                            sqlTransaction.Commit();
                        }
                        grid.Refresh();
                        grid.alertSuccess("Data deleted successfully");
                    }
                    break;                
            }
        }

        protected void grid_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            grid = (ASPxGridView)sender;
            grid.initializeCustomButtonCallback(e);

            List<object> selected = grid.GetSelectedKey("role_id");

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
                                M_Role.Enable(new M_Role { role_id = item.ToString().ToInteger() }, cnn, sqlTransaction);
                            }
                            Log.Insert(Log.LogType.ENABLE, "Enable role", JObject.FromObject(new { role_id = selected }), cnn, sqlTransaction);
                            sqlTransaction.Commit();
                            grid.Refresh();
                            break;
                        case "BUTTONDISABLE":
                            foreach (var item in selected)
                            {
                                M_Role.Disable(new M_Role { role_id = item.ToString().ToInteger() }, cnn, sqlTransaction);
                            }
                            Log.Insert(Log.LogType.DISABLE, "Disable role", JObject.FromObject(new { role_id = selected }), cnn, sqlTransaction);
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
                if (M_Role.IsExist(new M_Role { role_name = e.NewValues["role_name"].ToString() }))
                {
                    AddError(e.Errors, grid.Columns["role_name"], "* Data exist");
                }
            }
            else
            {
                if (e.NewValues["role_name"].ToString().Trim() != e.OldValues["role_name"].ToString().Trim())
                {
                    if (M_Role.IsExist(new M_Role { role_name = e.NewValues["role_name"].ToString() }))
                    {
                        AddError(e.Errors, grid.Columns["role_name"], "* Data exist");
                    }
                }
            }
                
        }

        protected void grid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            grid = (ASPxGridView)sender;
            using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
            {
                cnn.Open();
                using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                {
                    M_Role data = new M_Role();
                    data.role_name = (string)e.NewValues["role_name"].ToString();
                    data.role_desc = (string)e.NewValues["role_desc"] ?? string.Empty;
                    data.allow_create= e.NewValues["allow_create"].ToBoolean();
                    data.allow_update = e.NewValues["allow_update"].ToBoolean();
                    data.allow_delete = e.NewValues["allow_delete"].ToBoolean();
                    data.allow_export = e.NewValues["allow_export"].ToBoolean();
                    data.allow_import = e.NewValues["allow_import"].ToBoolean();
                    data.allow_enabledisable = e.NewValues["allow_enabledisable"].ToBoolean();
                    try
                    {
                        data = M_Role.Insert(data, cnn, sqlTransaction);
                        e.NewValues.Add(nameof(data.role_id), data.role_id);
                        Log.Insert(Log.LogType.ADD, "Add role", JObject.FromObject(e.NewValues), cnn, sqlTransaction);
                        sqlTransaction.Commit();
                        grid.alertSuccess();
                    }
                    catch (Exception ex)
                    {
                        grid.alertError(ex.Message);
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

        protected void grid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            grid = (ASPxGridView)sender;
            using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
            {
                cnn.Open();
                using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                {
                    M_Role data = new M_Role();
                    data.role_id = e.Keys["role_id"].ToInteger();
                    data.role_name = (string)e.NewValues["role_name"].ToString();
                    data.role_desc = (string)e.NewValues["role_desc"] ?? string.Empty;
                    data.allow_create = e.NewValues["allow_create"].ToString().ToBoolean();
                    data.allow_update = e.NewValues["allow_update"].ToString().ToBoolean();
                    data.allow_delete = e.NewValues["allow_delete"].ToString().ToBoolean();
                    data.allow_export = e.NewValues["allow_export"].ToString().ToBoolean();
                    data.allow_import = e.NewValues["allow_import"].ToString().ToBoolean();
                    data.allow_enabledisable = e.NewValues["allow_enabledisable"].ToString().ToBoolean();

                    try
                    {
                        M_Role.Update(data, cnn, sqlTransaction);
                        e.NewValues.Add(nameof(data.role_id), data.role_id);
                        Log.Insert(Log.LogType.UPDATE, "Update role", JObject.FromObject(e.NewValues), cnn, sqlTransaction);
                        sqlTransaction.Commit();
                        grid.alertSuccess();
                    }
                    catch (Exception ex)
                    {
                        grid.alertError(ex.Message);
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

        protected void grid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            grid = sender as ASPxGridView;
            grid.ShowDeleteConfirm(e);
        }
    }
}