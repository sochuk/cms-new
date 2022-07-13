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
using Oracle.ManagedDataAccess.Client;

namespace CMS.Management
{
    public partial class Module : CPanel
    {
        public DataTable dt = new DataTable();
        public DataTable module_root = new DataTable();

        public DataTable dt_temp = new DataTable();
        string[] column_name = new string[] { "Module Name", "Description" };
        string[] row_field = new string[] { "module_name", "module_desc" };

        [PrincipalPermission(SecurityAction.Demand, Role = "Super Administrator")]
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();

            grid.bindDelete(grid_delete, dt_temp, column_name, row_field);

            grid.InitNewRow += (obj, arg) =>
            {
                grid = (ASPxGridView)obj;
                arg.NewValues["isvisible"] = true;
            };
        }

        public void LoadData()
        {
            (grid.Columns["module_root"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_Module.SelectAll();
            (grid.Columns["module_icon"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_Icon.SelectAll();
            (grid.Columns["createby"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_User.BindData();
            (grid.Columns["updateby"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_User.BindData();

            grid.DataSource = M_Module.SelectAll();
            grid.DataBind();            

            ApplyGroup(0);
        }

        protected void grid_Init(object sender, EventArgs e)
        {
            grid = (ASPxGridView)sender;
            grid.initializeGrid();
        }

        private void DeleteSessionMenu()
        {
            HttpContext.Current.Session["M_Module_Root"] = null;
            HttpContext.Current.Session["M_Module"] = null;
        }

        protected void grid_CustomButtonInitialize(object sender, DevExpress.Web.ASPxGridViewCustomButtonEventArgs e)
        {
            grid = (ASPxGridView)sender;
            grid.initializeCustomButton(e);
        }

        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            grid = (ASPxGridView)sender;
            grid.initializeHtmlRowPrepared(e);
        }

        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            grid = (ASPxGridView)sender;
            //Action filter row in ASPXGridView
            grid.initializeCustomCallback(e);

            List<object> selected = grid.GetSelectedKey("module_id");

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
                                M_Module.Delete(new M_Module { module_id = id.ToInteger() }, cnn, sqlTransaction);
                                Log.Insert(Log.LogType.DELETE,
                                    "Delete m_module",
                                    JObject.FromObject(new { module_id = id.ToInteger() }),
                                    cnn,
                                    sqlTransaction);
                            }
                            sqlTransaction.Commit();

                            DeleteSessionMenu();
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

            List<object> selected = grid.GetSelectedKey("module_id");

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
                                M_Module.Enable(new M_Module { module_id = item.ToString().ToInteger() }, cnn, sqlTransaction);                                
                            }

                            Log.Insert(Log.LogType.ENABLE,
                                    "Enable m_module",
                                    JObject.FromObject(new { module_id = selected }),
                                    cnn,
                                    sqlTransaction);

                            sqlTransaction.Commit();

                            DeleteSessionMenu();

                            grid.Refresh();
                            break;
                        case "BUTTONDISABLE":
                            foreach (var item in selected)
                            {
                                M_Module.Disable(new M_Module { module_id = item.ToString().ToInteger() }, cnn, sqlTransaction);                                
                            }

                            Log.Insert(Log.LogType.DISABLE,
                                    "Disable m_module",
                                    JObject.FromObject(new { module_id = selected }),
                                    cnn,
                                    sqlTransaction);

                            sqlTransaction.Commit();
                            DeleteSessionMenu();

                            grid.Refresh();
                            break;
                    }

                }
            }
        }

        protected void grid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            grid = (ASPxGridView)sender;
            ASPxGridLookup root = grid.FindID("module_root", "lookup_module");
            e.NewValues["module_root"] = root.Value;
            M_Module module = new M_Module()
            {
                type_code = (string)e.NewValues["type_code"],
                module_name = (string)e.NewValues["module_name"],
                module_root = e.NewValues["module_root"] == null ? 0 : (int)e.NewValues["module_root"]
            };

            if (grid.IsNewRowEditing)
            {                
                if (M_Module.IsExist(module))
                {
                    AddError(e.Errors, grid.Columns["type_code"], "* Data exist");
                    root.IsValid = false;
                    root.ErrorText = "* Data exist";
                }
            }
            else
            {
                if (e.NewValues["type_code"].ToString().Trim() != e.OldValues["type_code"].ToString().Trim())
                {
                    if (M_Module.IsExist(module))
                    {
                        AddError(e.Errors, grid.Columns["type_code"], "* Data exist");
                        root.IsValid = false;
                        root.ErrorText = "* Data exist";
                    }
                }
            }

            string module_url = (string)e.NewValues["module_url"] ?? string.Empty;
            if (!module_url.Trim().StartsWith("/") && (module_url == null || module_url != ""))
            {
                AddError(e.Errors, grid.Columns["module_url"], "URL must start with \"/\". Example: \"/transaction\"");
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
                    ASPxGridLookup module_root = grid.FindID("module_root", "lookup_module");
                    ASPxGridLookup module_icon = grid.FindID("module_icon", "lookup_icon");
                    e.NewValues["module_root"] = module_root.Value;
                    e.NewValues["module_icon"] = module_icon.Value;                    

                    M_Module module = new M_Module();
                    module.type_code = e.NewValues["type_code"].ToString();
                    module.module_name = e.NewValues["module_name"].ToString();
                    module.module_desc = (string)e.NewValues["module_desc"] ?? string.Empty;
                    module.module_root = e.NewValues["module_root"] == null ? 0 : (int)e.NewValues["module_root"];
                    module.module_icon = (string)e.NewValues["module_icon"] ?? string.Empty;
                    module.module_url = (string)e.NewValues["module_url"] ?? string.Empty;
                    module.module_title = (string)e.NewValues["module_title"] ?? string.Empty;
                    module.isvisible = (bool)(e.NewValues["isvisible"]  ?? false);
                    module.order_no = e.NewValues["order_no"] == null ? 0 : (int)e.NewValues["order_no"];

                    try
                    {
                        M_Module.Insert(module, cnn, sqlTransaction);
                        Log.Insert(Log.LogType.ADD,
                                    "Add new m_module",
                                    JObject.FromObject(e.NewValues),
                                    cnn,
                                    sqlTransaction);

                        sqlTransaction.Commit();
                        grid.alertSuccess();

                        //Clear session M_Module
                        HttpContext.Current.Session["M_Module"] = null;
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
                    ASPxGridLookup module_root = grid.FindID("module_root", "lookup_module");
                    ASPxGridLookup module_icon = grid.FindID("module_icon", "lookup_icon");
                    e.NewValues["module_root"] = module_root.Value;
                    e.NewValues["module_icon"] = module_icon.Value;

                    M_Module module = new M_Module();
                    module.module_id = (int)e.Keys["module_id"];
                    module.type_code = e.NewValues["type_code"].ToString();
                    module.module_name = e.NewValues["module_name"].ToString();
                    module.module_desc = (string)e.NewValues["module_desc"] ?? string.Empty;
                    module.module_root = e.NewValues["module_root"] == null ? 0 : (int)e.NewValues["module_root"];
                    module.module_icon = (string)e.NewValues["module_icon"] ?? string.Empty;
                    module.module_url = (string)e.NewValues["module_url"] ?? string.Empty;
                    module.module_title = (string)e.NewValues["module_title"] ?? string.Empty;
                    module.isvisible = (bool)(e.NewValues["isvisible"] ?? false);
                    module.order_no = e.NewValues["order_no"] == null ? 0 : (int)e.NewValues["order_no"];

                    try
                    {
                        M_Module.Update(module, cnn, sqlTransaction);
                        Log.Insert(Log.LogType.UPDATE,
                                    "Update m_module",
                                    JObject.FromObject(e.NewValues),
                                    cnn,
                                    sqlTransaction);

                        sqlTransaction.Commit();
                        LoadData();
                        grid.alertSuccess();
                        (sender as ASPxGridView).CancelEdit();

                        //Clear session M_Module
                        HttpContext.Current.Session["M_Module"] = null;
                    }
                    catch (Exception ex)
                    {
                        grid.alertError(ex.Message);
                        sqlTransaction.Rollback();
                        throw new Exception(ex.Message);
                    }                    
                }
                cnn.Close();
            }
            e.Cancel = true;
        }

        void ApplyGroup(int ColumnIndex)
        {
            grid.BeginUpdate();
            try
            {
                grid.ClearSort();
                switch (ColumnIndex)
                {
                    case 0:
                        grid.GroupBy(grid.Columns["module_name_group"]);
                        break;
                }
            }
            finally
            {
                grid.EndUpdate();
            }
            //grid.ExpandAll();
        }

        protected void Module_Root_Init(object sender, EventArgs e)
        {
            module_root = Database.getDataTable(@"SELECT * FROM m_module ");
            (sender as ASPxGridLookup).DataSource = module_root;
        }

        protected void grid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName != "module_icon") return;
            string icon = e.GetFieldValue("module_icon") == DBNull.Value ? string.Empty : (string)e.GetFieldValue("module_icon");
            if (icon != null || icon == string.Empty || icon == "")
            {
                e.DisplayText = "<i class=\""+ icon + "\"></i>";
            }            
        }

        protected void module_icon_Init(object sender, EventArgs e)
        {            
            DataTable icon = new DataTable();
            icon = M_Icon.SelectAll();
            (sender as ASPxGridLookup).DataSource = icon;
        }

        protected void lookup_module_Init(object sender, EventArgs e)
        {
            ASPxGridLookup lookup = sender as ASPxGridLookup;
            lookup.DataSource = M_Module.SelectAll();
            lookup.Bind("module_root");
        }

        protected void lookup_icon_Init(object sender, EventArgs e)
        {
            ASPxGridLookup lookup = sender as ASPxGridLookup;
            lookup.DataSource = M_Icon.SelectAll();
            lookup.Bind("module_icon");
        }

        protected void grid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            grid = sender as ASPxGridView;
            grid.ShowDeleteConfirm(e);
        }
    }
}