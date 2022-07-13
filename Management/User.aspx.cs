using DevExpress.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CMS.Helper;
using CMS.Management.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace CMS.Management
{
    [NeedAccessRight]
    public partial class User : CPanel
    {
        public DataTable dt = new DataTable();
        public DataTable company = new DataTable();
        public DataTable theme = new DataTable();
        public DataTable group = new DataTable();
        public DataTable gender = new DataTable();

        public DataTable dt_temp = new DataTable();
        string[] column_name = new string[] { "Username", "Full Name" };
        string[] row_field = new string[] { "username", "fullname" };

        [PrincipalPermission(SecurityAction.Demand, Role = "Super Administrator")]
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
            grid.bindDelete(grid_delete, dt_temp, column_name, row_field);            
        }
        
        public void LoadData()
        {
            gender = typeof(M_Gender.Gender).ToDataTable("gender_id", "gender_desc");

            (grid.Columns["gender"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = gender;
            (grid.Columns["group_id"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_Group.SelectAll();
            (grid.Columns["theme_id"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_Theme.SelectAll();
            (grid.Columns["company_id"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_Company_Core.SelectAll();
            (grid.Columns["createby"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_User.BindData();
            (grid.Columns["updateby"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_User.BindData();

            dt = M_User.SelectAll();
            grid.DataSource = dt;
            grid.DataBind();
        }

        protected void grid_Init(object sender, EventArgs e)
        {
            grid = (ASPxGridView)sender;
            grid.initializeGrid();
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

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            (sender as ASPxGridView).DataSource = dt;            
        }

        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            grid = (ASPxGridView)sender;
            grid.initializeCustomCallback(e);

            List<object> selected = grid.GetSelectedKey("user_id");

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
                                var user = new M_User { user_id = id.ToInteger() };
                                M_User.Delete(user, cnn, sqlTransaction);                                
                            }
                            Log.Insert(Log.LogType.DELETE, "Delete user", JObject.FromObject(new { user_id = selected }), cnn, sqlTransaction);
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

            List<object> selected = grid.GetSelectedKey("user_id");

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
                                var user = new M_User { user_id = item.ToString().ToInteger() };
                                M_User.Enable(user, cnn, sqlTransaction);                                
                            }
                            Log.Insert(Log.LogType.ENABLE, "Enable user", JObject.FromObject(new { user_id = selected }), cnn, sqlTransaction);
                            sqlTransaction.Commit();
                            grid.Refresh();
                            break;

                        case "BUTTONDISABLE":
                            foreach (var item in selected)
                            {
                                var user = new M_User { user_id = item.ToString().ToInteger() };
                                M_User.Disable(user, cnn, sqlTransaction);                                
                            }
                            Log.Insert(Log.LogType.DISABLE, "Disable user", JObject.FromObject(new { user_id = selected }), cnn, sqlTransaction);
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
                if (M_User.IsExist(new M_User { username = e.NewValues["username"].ToString() }))
                {
                    AddError(e.Errors, grid.Columns["username"], "* Data exist");
                }
            }
            else
            {
                if (e.NewValues["username"].ToString().Trim() != e.OldValues["username"].ToString().Trim())
                {
                    if (M_User.IsExist(new M_User { username = e.NewValues["username"].ToString() }))
                    {
                        AddError(e.Errors, grid.Columns["username"], "* Data exist");
                    }
                }
            }
        }

        protected void grid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        { 
            using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
            {
                cnn.Open();
                using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                {
                    try
                    {
                        ASPxGridLookup group = grid.FindID("group_id", "lookup_group");
                        ASPxGridLookup gender = grid.FindID("gender", "lookup_gender");
                        ASPxGridLookup theme = grid.FindID("theme_id", "lookup_theme");
                        ASPxGridLookup company = grid.FindID("company_id", "lookup_company");
                        e.NewValues["group_id"] = group.Value;
                        e.NewValues["gender"] = gender.Value;
                        e.NewValues["theme_id"] = theme.Value;
                        e.NewValues["company_id"] = company.Value;

                        string password = Crypto.ComputeSha256Hash(Crypto.Encode64Byte(e.NewValues["username"].ToString()));

                        M_User user = new M_User();
                        user.username = e.NewValues["username"].ToString();
                        user.fullname = (string)e.NewValues["fullname"] ?? string.Empty;
                        user.email = (string)e.NewValues["email"] ?? string.Empty;
                        user.phone = PhoneNumber((string)e.NewValues["phone"] ?? string.Empty);
                        user.notes = (string)e.NewValues["notes"] ?? string.Empty;
                        user.gender = (int)e.NewValues["gender"];
                        user.theme_id = (int)e.NewValues["theme_id"];
                        user.group_id = (int)e.NewValues["group_id"];
                        user.password = password;
                        user.company_id = (int)e.NewValues["company_id"];
                        user.isrequired_token = e.NewValues["isrequired_token"] == null ? false : (bool)e.NewValues["isrequired_token"];
                        user.telegram_id = e.NewValues["telegram_id"] == null ? string.Empty : (string)e.NewValues["telegram_id"];

                        user = M_User.Insert(user, cnn, sqlTransaction);
                        e.NewValues.Add(nameof(user.user_id), user.user_id);

                        Log.Insert(Log.LogType.ADD, 
                            "Add new m_user", 
                            JObject.FromObject(e.NewValues, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore }), 
                            cnn, 
                            sqlTransaction);

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

            (sender as ASPxGridView).CancelEdit();
            e.Cancel = true;
        }

        protected void grid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            grid = (ASPxGridView)sender;
            try
            {               
                string directoryName = Server.MapPath("~/Media/User/Images/");
                string fileName = directoryName + Crypto.RandomString(16) + ".jpg";

                ASPxBinaryImage Photo = (ASPxBinaryImage)grid.FindEditFormTemplateControl("photo");
                HiddenField hPhoto = (HiddenField)grid.FindEditFormTemplateControl("hPhoto");
                var old_img = Converter.GetByteArrayFromImage(Server.MapPath(hPhoto.Value.ToString()));

                ASPxGridLookup group = grid.FindID("group_id", "lookup_group");
                ASPxGridLookup gender = grid.FindID("gender", "lookup_gender");
                ASPxGridLookup theme = grid.FindID("theme_id", "lookup_theme");
                ASPxGridLookup company = grid.FindID("company_id", "lookup_company");
                e.NewValues["group_id"] = group.Value;
                e.NewValues["gender"] = gender.Value;
                e.NewValues["theme_id"] = theme.Value;
                e.NewValues["company_id"] = company.Value;


                M_User user = new M_User();
                user.user_id = (int)e.Keys["user_id"].ToString().ToInteger();
                user.username = e.OldValues["username"].ToString();
                user.fullname = (string)e.NewValues["fullname"] ?? string.Empty;
                user.email = (string)e.NewValues["email"] ?? string.Empty;
                user.phone = PhoneNumber((string)e.NewValues["phone"] ?? string.Empty);
                user.notes = (string)e.NewValues["notes"] ?? string.Empty;
                user.gender = (int)e.NewValues["gender"];
                user.theme_id = (int)e.NewValues["theme_id"];
                user.group_id = (int)e.NewValues["group_id"];
                user.company_id = (int)e.NewValues["company_id"];
                user.isrequired_token = e.NewValues["isrequired_token"] == null ? false : (bool)e.NewValues["isrequired_token"];
                user.telegram_id = e.NewValues["telegram_id"] == null ? string.Empty : (string)e.NewValues["telegram_id"];

                if (Photo.ContentBytes.SequenceEqual(old_img))
                {
                    user.photo = hPhoto.Value.ToString();
                }
                else
                {
                    user.photo = (Photo.ContentBytes == null ? string.Empty : MapURL(fileName));
                }                
                
                //Save Image
                try
                {
                    if (Photo.ContentBytes != null && !Photo.ContentBytes.SequenceEqual(old_img) )
                    {
                        using (MemoryStream img_stream = new MemoryStream(Photo.ContentBytes))
                        {
                            using (System.Drawing.Image image = System.Drawing.Image.FromStream(img_stream))
                            {
                                if (!System.IO.Directory.Exists(directoryName))
                                {
                                    System.IO.Directory.CreateDirectory(directoryName);
                                }
                                var img = Converter.ResizeImage(image);
                                Converter.SaveAsJpeg(img, fileName);
                            }
                        }
                    }
                    
                }
                catch(Exception ex)
                {
                    ex.Message.ToString();
                }
                

                using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
                {
                    cnn.Open();                    

                    using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                    {
                        try
                        {
                            M_User.Update(user, cnn, sqlTransaction);
                            M_User.UpdatePhoto(user, cnn, sqlTransaction);
                            M_User.UpdateNote(user, cnn, sqlTransaction);

                            e.NewValues.Add(nameof(user.user_id), user.user_id);
                            Log.Insert(Log.LogType.UPDATE, 
                                "Update m_user", 
                                JObject.FromObject(e.NewValues, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore }), 
                                cnn, 
                                sqlTransaction);

                            sqlTransaction.Commit();
                            grid.alertSuccess();
                        }
                        catch(Exception ex)
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
                
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
            }
            
            e.Cancel = true;            
        }

        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            grid = (ASPxGridView)sender;
            if (grid.IsNewRowEditing) return;
            if (e.Column.FieldName == "username") e.Editor.ReadOnly = true;
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

        protected void lookup_group_Init(object sender, EventArgs e)
        {
            ASPxGridLookup lookup = sender as ASPxGridLookup;
            lookup.DataSource = M_Group.SelectAll();
            lookup.Bind("group_id");
        }

        protected void lookup_gender_Init(object sender, EventArgs e)
        {
            ASPxGridLookup lookup = sender as ASPxGridLookup;
            lookup.DataSource = typeof(M_Gender.Gender).ToDataTable("gender_id", "gender_desc"); ;
            lookup.Bind("gender");
        }

        protected void lookup_theme_Init(object sender, EventArgs e)
        {
            ASPxGridLookup lookup = sender as ASPxGridLookup;
            lookup.DataSource = M_Theme.SelectAll();
            lookup.Bind("theme_id");
        }

        protected void lookup_company_Init(object sender, EventArgs e)
        {
            ASPxGridLookup lookup = sender as ASPxGridLookup;
            lookup.DataSource = M_Company_Core.SelectAll();
            lookup.Bind("company_id");
        }

        protected void grid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            grid = sender as ASPxGridView;
            grid.ShowDeleteConfirm(e);
        }

    }
}