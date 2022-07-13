using DevExpress.Web;
using Newtonsoft.Json.Linq;
using KMS.Helper;
using KMS.Management.Model;
using KMS.Management.Structure.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KMS.Management.Structure
{
    [NeedAccessRight]
    public partial class Division : CPanel
    {
        public DataTable dt = new DataTable();
        public DataTable dt_temp = new DataTable();

        string[] column_name = new string[] { "Division Name", "Description" };
        string[] row_field = new string[] { "division_name", "division_desc" };

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
            grid.bindDelete(grid_delete, dt_temp, column_name, row_field);
        }

        public void LoadData()
        {
            dt = M_Division.SelectAll();
            (grid.Columns["department_id"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_Department.SelectAll();
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

            List<object> selected = grid.GetSelectedKey("division_id");
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
                                M_Division.Enable(new M_Division { division_id = item.ToString().ToInteger() }, cnn, sqlTransaction);
                            }
                            Log.Insert(Log.LogType.ENABLE, "Enable division", JObject.FromObject(new { division_id = selected }), cnn, sqlTransaction);
                            sqlTransaction.Commit();
                            grid.Refresh();
                            break;
                        case "BUTTONDISABLE":
                            foreach (var item in selected)
                            {
                                M_Division.Disable(new M_Division { division_id = item.ToString().ToInteger() }, cnn, sqlTransaction);
                            }
                            Log.Insert(Log.LogType.DISABLE, "Disable division", JObject.FromObject(new { division_id = selected }), cnn, sqlTransaction);
                            sqlTransaction.Commit();
                            grid.Refresh();
                            break;
                    }

                }
            }
        }

        protected void grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            grid = (ASPxGridView)sender;
            grid.initializeCustomCallback(e);

            List<object> selected = grid.GetSelectedKey("division_id");

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
                                M_Division.Delete(new M_Division { division_id = id.ToInteger() }, cnn, sqlTransaction);
                            }
                            Log.Insert(Log.LogType.DELETE, "Delete division", JObject.FromObject(new { division_id = selected }), cnn, sqlTransaction);
                            sqlTransaction.Commit();
                        }
                        grid.Refresh();
                    }
                    break;
            }
        }

        protected void grid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            grid = (ASPxGridView)sender;
            if (grid.IsNewRowEditing)
            {
                if (M_Division.IsExist(new M_Division { division_name = e.NewValues["division_name"].ToString() }))
                {
                    AddError(e.Errors, grid.Columns["division_name"], "* Data exist");
                }
            }
            else
            {
                if (e.NewValues["division_name"].ToString().Trim() != e.OldValues["division_name"].ToString().Trim())
                {
                    if (M_Division.IsExist(new M_Division { division_name = e.NewValues["division_name"].ToString() }))
                    {
                        AddError(e.Errors, grid.Columns["division_name"], "* Data exist");
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
                    M_Division data = new M_Division();
                    ASPxGridLookup department = grid.FindID("department_id", "lookup_department");
                    e.NewValues["department_id"] = department.Value;

                    data.division_name = (string)e.NewValues["division_name"];
                    data.division_desc = (string)e.NewValues["division_desc"];
                    data.department_id = (int)e.NewValues["department_id"];

                    try
                    {
                        data = M_Division.Insert(data, cnn, sqlTransaction);
                        e.NewValues.Add(nameof(data.division_id), data.division_id);
                        Log.Insert(Log.LogType.ADD, "Add division", JObject.FromObject(e.NewValues), cnn, sqlTransaction);
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
                    M_Division data = new M_Division();
                    ASPxGridLookup department = grid.FindID("department_id", "lookup_department");
                    e.NewValues["department_id"] = department.Value;

                    data.division_id = (int)e.Keys["division_id"];
                    data.division_name = (string)e.NewValues["division_name"];
                    data.division_desc = (string)e.NewValues["division_desc"];
                    data.department_id = (int)e.NewValues["department_id"];

                    try
                    {
                        M_Division.Update(data, cnn, sqlTransaction);
                        e.NewValues.Add(nameof(data.division_id), data.division_id);
                        Log.Insert(Log.LogType.UPDATE, "Update division", JObject.FromObject(e.NewValues), cnn, sqlTransaction);
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

        protected void lookup_department_Init(object sender, EventArgs e)
        {
            ASPxGridLookup lookup = sender as ASPxGridLookup;
            lookup.DataSource = M_Department.SelectAll();
            lookup.Bind("department_id");
        }

        protected void grid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            grid = sender as ASPxGridView;
            grid.ShowDeleteConfirm(e);
        }
    }
}