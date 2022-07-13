using DevExpress.Web;
using CMS.Helper;
using CMS.Management.Model;
using CMS.Notification;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace CMS.Account
{
    public partial class Setting : CPanel
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
            if (!IsPostBack)
            {
                Theme.DataBind();
                GridPreview.DataBind();

                var setting = M_User.getSetting();
                Theme.GridView.Selection.SelectRow(Convert.ToInt32(Enum.Parse(typeof(M_Setting.Theme), setting.grid_theme.ToString())));
                PageSize.Text = setting.grid_pagesize.ToString();
                Zebra.Value = setting.grid_zebracolor;
                WrapColumn.Value = setting.grid_wrap_column;
                WrapCell.Value = setting.grid_wrap_cell;
                FilterBar.Value = setting.grid_showfilterbar;
                ClickSelect.Value = setting.grid_selectbyrow;
                Focused.Value = setting.grid_focuserow;
                Ellipsis.Value = setting.grid_ellipsis;
                Footer.Value = setting.grid_showfooter;
                Responsive.Value = setting.grid_responsive;
            }

            if (IsPostBack && !IsCallback)
            {
                SaveSetting();
            }

        }

        public void LoadData()
        {
            Theme.DataSource = typeof(M_Setting.Theme).ToDataTable();           

            DataTable dt = new DataTable();
            dt.Columns.Add("No", typeof(int));
            for (int x = 1; x <= 5; x++)
            {
                dt.Columns.Add("Column " + x, typeof(string));
            }

            dt.Columns.Add("isactive", typeof(bool));

            for (int i = 1; i <= 99; i++)
            {
                dt.Rows.Add(new object[] { i.ToString() ,
                    "Column 1 Row " + i,
                    "Column 2 Row " + i,
                    "Column 3 Row " + i,
                    "Column 4 Row " + i,
                    "Column 5 Row " + i,
                    true
                });
            }

            GridPreview.DataSource = dt;
        }

        protected void GridPreview_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            GridPreview = (ASPxGridView)sender;
            GridPreview.initializeHtmlRowPrepared(e);
        }

        protected void GridPreview_Init(object sender, EventArgs e)
        {
            GridPreview = (ASPxGridView)sender;
            GridPreview.initializeGrid();
        }

        protected void GridPreview_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GridPreview = (ASPxGridView)sender;
            GridPreview.initializeCustomCallback(e); 
        }

        protected void GridPreview_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            GridPreview = (ASPxGridView)sender;
            GridPreview.initializeCustomButton(e);
        }

        protected void GridPreview_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            GridPreview = (ASPxGridView)sender;
            GridPreview.initializeCustomButtonCallback(e);
        }

        public void SaveSetting()
        {
            M_Setting setting = new M_Setting
            {
                user_id = HttpContext.Current.User.Identity.Get_Id().ToInteger(),
                grid_pagesize = PageSize.Text.ToInteger(),
                grid_theme = (M_Setting.Theme)Theme.Value,
                grid_zebracolor = Zebra.Checked,
                grid_wrap_column = WrapColumn.Checked,
                grid_wrap_cell = WrapCell.Checked,
                grid_showfilterrow = false,
                grid_showfilterbar = FilterBar.Checked,
                grid_selectbyrow = ClickSelect.Checked,
                grid_focuserow = Focused.Checked,
                grid_ellipsis = Ellipsis.Checked,
                grid_showfooter = Footer.Checked,
                grid_responsive = Responsive.Checked
        };

            using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
            {
                cnn.Open();
                using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                {

                    if (!M_Setting.IsExist(setting))
                    {
                        M_Setting.Insert(setting, cnn, sqlTransaction);
                    }
                    else
                    {
                        M_Setting.Update(setting, cnn, sqlTransaction);
                    }
                    sqlTransaction.Commit();
                }
            }

            Response.Redirect(Request.RawUrl);
        }

        protected void GridPreview_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
            Alert alert = new Alert("Information", "This is preview. You cant delete/remove data in this preview", Alert.TypeMessage.Success, Alert.PositionMessage.BottomRight);
            GridPreview.JSProperties["cpSuccess"] = alert.ToString();
        }

        protected void GridPreview_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            GridPreview = (ASPxGridView)sender;
            GridPreview.CancelEdit();
            e.Cancel = true;
            Alert alert = new Alert("Information", "This is preview. You cant edit/update data in this preview", Alert.TypeMessage.Success, Alert.PositionMessage.BottomRight);
            GridPreview.JSProperties["cpSuccess"] = alert.ToString();
        }

        protected void GridPreview_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            GridPreview = (ASPxGridView)sender;
            GridPreview.CancelEdit();
            e.Cancel = true;
            Alert alert = new Alert("Information", "This is preview. You cant insert/add data in this preview", Alert.TypeMessage.Success, Alert.PositionMessage.BottomRight);
            GridPreview.JSProperties["cpSuccess"] = alert.ToString();
        }
    }
}