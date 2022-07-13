using CMS.Helper;
using CMS.Management.Model;
using CMS.Notification;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace CMS.Account
{
    public partial class ChangePassword : CPanel
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Change Password";
        }

        protected void cPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            switch (e.Parameter.ToUpper())
            {
                case "SAVE":
                    string old_password = Crypto.EncryptPassword(old_passwords.Text.Trim());
                    string new_password = Crypto.EncryptPassword(new_passwords.Text.Trim());

                    DataTable dt = new DataTable();
                    dt = Database.getDataTable("SELECT Password FROM M_User WHERE user_id=:user_id",
                        new OracleParameter(":user_id", User.Identity.Get_Id()));

                    if (dt.Rows.Count > 0)
                    {
                        //Success
                        if (dt.Rows[0]["Password"].ToString() == old_password)
                        {
                            using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
                            {
                                cnn.Open();
                                using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                                {
                                    try
                                    {
                                        M_User user = new M_User
                                        {
                                            user_id = User.Identity.Get_Id().ToInteger(),
                                            password = new_password
                                        };

                                        M_User.ChangePassword(user, cnn, sqlTransaction);
                                        sqlTransaction.Commit();

                                        HttpContext.Current.User.Identity.UpdateClaim(UserClaim.Password, new_password);
                                        cPanel.alertSuccess("Change password successfully");
                                    }
                                    catch (Exception ex)
                                    {
                                        ex.Message.ToString();
                                        sqlTransaction.Rollback();
                                        throw new Exception(ex.Message);
                                    }
                                }
                                cnn.Close();
                            }
                        }
                        //Failed
                        else
                        {
                            cPanel.alertError("Old password didn't match.");
                        }
                    }
                    else
                    {
                        cPanel.alertError("Change password failed.");
                    }
                    break;
            }
        }
    }
}