using DevExpress.Web;
using Newtonsoft.Json.Linq;
using KMS.Helper;
using KMS.Management.Model;
using KMS.Management.Structure.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace KMS.Management.Structure
{
    [NeedAccessRight]
    public partial class Department : CPanel
    {
        public DataTable dt = new DataTable();
        public DataTable dt_temp = new DataTable();

        string[] column_name = new string[] { "Department Name", "Description" };
        string[] row_field = new string[] { "department_name", "department_desc" };

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
            grid.bindDelete(grid_delete, dt_temp, column_name, row_field);
        }

        public void LoadData()
        {
            dt = M_Department.SelectAll();

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

            List<object> selected = grid.GetSelectedKey("department_id");
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
                                M_Department.Enable(new M_Department { department_id = item.ToString().ToInteger() }, cnn, sqlTransaction);
                            }

                            Log.Insert(Log.LogType.ENABLE, "Enable department", JObject.FromObject(new { department_id = selected }), cnn, sqlTransaction);
                            sqlTransaction.Commit();
                            grid.Refresh();
                            break;
                        case "BUTTONDISABLE":
                            foreach (var item in selected)
                            {
                                M_Department.Disable(new M_Department { department_id = item.ToString().ToInteger() }, cnn, sqlTransaction);
                            }

                            Log.Insert(Log.LogType.DISABLE, "Disable department", JObject.FromObject(new { department_id = selected }), cnn, sqlTransaction);
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

            List<object> selected = grid.GetSelectedKey("department_id");

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
                                M_Department.Delete(new M_Department { department_id = id.ToInteger() }, cnn, sqlTransaction);
                            }

                            Log.Insert(Log.LogType.DELETE, "Delete department", JObject.FromObject(new { department_id = selected }), cnn, sqlTransaction);

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
                if (M_Department.IsExist(new M_Department { department_name = e.NewValues["department_name"].ToString() }))
                {
                    AddError(e.Errors, grid.Columns["department_name"], "* Data exist");
                }
            }
            else
            {
                if (e.NewValues["department_name"].ToString().Trim() != e.OldValues["department_name"].ToString().Trim())
                {
                    if (M_Department.IsExist(new M_Department { department_name = e.NewValues["department_name"].ToString() }))
                    {
                        AddError(e.Errors, grid.Columns["department_name"], "* Data exist");
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
                    M_Department data = new M_Department();
                    data.department_name = (string)e.NewValues["department_name"];
                    data.department_desc = (string)e.NewValues["department_desc"];

                    try
                    {
                        data = M_Department.Insert(data, cnn, sqlTransaction);
                        e.NewValues.Add(nameof(data.department_id), data.department_id);
                        Log.Insert(Log.LogType.ADD, "Add department", JObject.FromObject(e.NewValues), cnn, sqlTransaction);
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
                    M_Department data = new M_Department();
                    data.department_id = (int)e.Keys["department_id"];
                    data.department_name = (string)e.NewValues["department_name"];
                    data.department_desc = (string)e.NewValues["department_desc"];

                    try
                    {
                        M_Department.Update(data, cnn, sqlTransaction);
                        e.NewValues.Add(nameof(data.department_id), data.department_id);
                        Log.Insert(Log.LogType.UPDATE, "Add department", JObject.FromObject(e.NewValues), cnn, sqlTransaction);
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

        protected void grid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            grid = sender as ASPxGridView;
            grid.ShowDeleteConfirm(e);
        }
    }
}