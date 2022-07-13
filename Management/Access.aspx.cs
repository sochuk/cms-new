using DevExpress.Web;
using Newtonsoft.Json.Linq;
using CMS.Helper;
using CMS.Management.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class Access : CPanel
    {
        public DataTable dt = new DataTable();
        public DataTable module = new DataTable();

        [PrincipalPermission(SecurityAction.Demand, Role = "Super Administrator")]
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        public void LoadData()
        {
            dt = Database.getDataTable(@"
                SELECT G.*,R.role_name,R.role_desc FROM m_group  G
                LEFT JOIN m_role  R ON G.role_id=R.role_id").ToColumnLowerCase();

            grid.DataSource = dt;
            grid.DataBind();            

            module = Database.getDataTable(@"
                SELECT A.*, B.module_name module_name_group FROM m_module  A
                LEFT JOIN m_module  B ON A.module_root=B.module_id
                ").ToColumnLowerCase();

            Grid_Access.DataSource = module;
            Grid_Access.DataBind();
            Grid_Access.BeginUpdate();
            Grid_Access.ClearSort();
            Grid_Access.GroupBy(Grid_Access.Columns["module_name_group"]);
            Grid_Access.EndUpdate();
            Grid_Access.ExpandAll();
        }

        protected void grid_ToolbarItemClick(object source, DevExpress.Web.Data.ASPxGridViewToolbarItemClickEventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)source;

            switch (e.Item.Name.ToUpper())
            {
                case "REFRESH":
                    grid.CancelEdit();
                    grid.DataSource = dt;
                    grid.DataBind();
                    break;

                case "GROUP":
                    ApplyGroup(0);
                    break;

                case "ROLE":
                    ApplyGroup(1);
                    break;

                case "NONE":
                    grid.ClearSort();
                    ReadOnlyCollection<GridViewDataColumn> groupCol = grid.GetGroupedColumns();
                    foreach (GridViewDataColumn col in groupCol)
                    {
                        col.GroupIndex = -1;
                    }                    
                    break;

                default:
                    break;
            }
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
                        grid.GroupBy(grid.Columns["group_name"]);
                        break;
                    case 1:
                        grid.GroupBy(grid.Columns["role_name"]);
                        break;
                }
            }
            finally
            {
                grid.EndUpdate();
            }
            grid.ExpandAll();
        }

        protected void Grid_Access_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            Grid_Access = (ASPxGridView)sender;
            DataTable access = new DataTable();
            access = Database.getDataTable("SELECT * FROM m_access  WHERE group_id=:group_id",
                new OracleParameter(":group_id", e.Parameters.ToString().Trim()));

            Grid_Access.ExpandAll();
            Grid_Access.Selection.UnselectAll();
            for (int i = 0; i < Grid_Access.VisibleRowCount; i++)
            {                
                foreach (DataRow row in access.Rows)
                {
                    var a = Grid_Access.GetRowValues(i, "module_id").ToString();
                    var b = row["module_id"].ToString();
                    if (a == b) Grid_Access.Selection.SelectRow(i);
                }                
            }
        }

        protected void Grid_Access_DataBinding(object sender, EventArgs e)
        {
            Grid_Access = (ASPxGridView)sender;
        }

        protected void grid_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;
            grid = (ASPxGridView)sender;
            grid.initializeCustomButton(e);
            
            if (e.ButtonID == "Access_Right_Button")
            {
                string role = grid.GetRowValues(e.VisibleIndex, "role_name").ToString();
                if (role == "Super Administrator") e.Visible = DevExpress.Utils.DefaultBoolean.False;
            }
            
        }

        protected void grid_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            grid = (ASPxGridView)sender;
            grid.Selection.UnselectAll();
            grid.Selection.SelectRow(e.VisibleIndex);
            grid.JSProperties["cpGroup_Id"] = grid.GetSelectedFieldValues("group_id");
            grid.JSProperties["cpShowPopup"] = true;
        }

        protected void cpCallback_Callback(object sender, CallbackEventArgsBase e)
        {
            var param = e.Parameter.Split(',');
            //Index of 0 array from grid
            int group_id = Convert.ToInt32(param.Where((source, index) => index == 0).Select(source => source).FirstOrDefault());
            //Index of 1 until end array from Grid_Access
            var access_id = param.Where((source, index) => index != 0).ToArray();

            using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
            {
                cnn.Open();
                using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                {
                    try
                    {
                        //Delete Access by User_Id
                        M_Access.Delete(new M_Access { group_id = group_id }, cnn, sqlTransaction);

                        List<M_Access> list = new List<M_Access>();
                        foreach (string module_id in access_id)
                        {
                            M_Access access = new M_Access
                            {
                                module_id = module_id.ToInteger(),
                                group_id = group_id
                            };

                            list.Add(access);
                            M_Access.Insert(access, cnn, sqlTransaction);
                        }

                        Log.Insert(Log.LogType.UPDATE,
                                    "Add/update/remove m_access",
                                    JObject.FromObject(new { data = list }),
                                    cnn,
                                    sqlTransaction);

                        sqlTransaction.Commit();
                        cpCallback.alertSuccess();
                    }
                    catch (Exception ex)
                    {
                        ex.Message.ToString();
                        sqlTransaction.Rollback();
                        cpCallback.alertError(ex.Message.ToString());
                        throw new Exception(ex.Message);
                    }
                }
                cnn.Close();
            }
            Access_Control.ShowOnPageLoad = false;
        }

        protected void grid_Init(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            grid.initializeGrid();
        }
    }
}