using DevExpress.Web;
using CMS.Helper;
using CMS.Management.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace CMS.Account
{
    public partial class Profile : CPanel
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable group = M_Group.SelectAll().ToColumnLowerCase();
            DataTable theme = M_Theme.SelectAll().ToColumnLowerCase();
            DataTable gender = typeof(M_Gender.Gender).ToDataTable("gender_id", "gender_desc").ToColumnLowerCase();
            DataTable company = M_Company_Core.SelectAll().ToColumnLowerCase();

            (grid.Columns["theme_id"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = theme;
            (grid.Columns["gender"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = gender;

            DataTable dt = M_User.SelectCurrentUser();

            grid.DataSource = dt;
            grid.DataBind();
        }

        protected void grid_Init(object sender, EventArgs e)
        {
            grid = (ASPxGridView)sender;
            //grid.initializeGrid();
        }

        protected void lookup_group_Init(object sender, EventArgs e)
        {
            ASPxGridLookup lookup = sender as ASPxGridLookup;
            lookup.DataSource = M_Group.SelectAll().ToColumnLowerCase();
            lookup.Bind("group_id");
        }

        protected void lookup_gender_Init(object sender, EventArgs e)
        {
            ASPxGridLookup lookup = sender as ASPxGridLookup;
            lookup.DataSource = typeof(M_Gender.Gender).ToDataTable("gender_id", "gender_desc").ToColumnLowerCase();
            lookup.Bind("gender");
        }

        protected void lookup_theme_Init(object sender, EventArgs e)
        {
            ASPxGridLookup lookup = sender as ASPxGridLookup;
            lookup.DataSource = M_Theme.SelectAll().ToColumnLowerCase();
            lookup.Bind("theme_id");
        }

        protected void lookup_company_Init(object sender, EventArgs e)
        {
            ASPxGridLookup lookup = sender as ASPxGridLookup;
            lookup.DataSource = M_Company_Core.SelectAll().ToColumnLowerCase();
            lookup.Bind("company_id");
        }

        protected void grid_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            grid = (ASPxGridView)sender;
            grid.initializeCustomButtonCallback(e);
        }

        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            grid = (ASPxGridView)sender;
            grid.initializeHtmlRowPrepared(e);
        }

        protected void grid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            grid = (ASPxGridView)sender;
            try
            {
                ASPxBinaryImage Photo = (ASPxBinaryImage)grid.FindEditFormTemplateControl("photo");

                M_User user = new M_User();
                user.user_id = (int)e.Keys["user_id"].ToString().ToInteger();
                user.username = e.OldValues["username"].ToString();
                user.fullname = (string)e.NewValues["fullname"] ?? string.Empty;
                user.email = (string)e.NewValues["email"] ?? string.Empty;
                user.phone = PhoneNumber((string)e.NewValues["phone"] ?? string.Empty);
                user.gender = e.NewValues["gender"].ToInteger();
                user.theme_id = e.NewValues["theme_id"].ToInteger();               

                using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
                {
                    cnn.Open();

                    using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                    {
                        try
                        {
                            M_User.UpdateProfile(user, cnn, sqlTransaction);
                            M_User.UpdatePhoto(user, Photo.ContentBytes, cnn, sqlTransaction);

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
                        grid.DataSource = M_User.SelectCurrentUser();
                    }
                    cnn.Close();
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            e.Cancel = true;
        }

        private string MapURL(string path)
        {
            string appPath = Server.MapPath("/").ToLower();
            return string.Format("/{0}", path.ToLower().Replace(appPath, "").Replace(@"\", "/"));
        }

        private string PhoneNumber(string str)
        {
            return (str.Trim() == "+  -    -    -") ? string.Empty : str.ToString().Trim();
        }

        
    }
}