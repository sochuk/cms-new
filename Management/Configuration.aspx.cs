using CMS.Helper;
using CMS.Management.Model;
using CMS.Management.Structure.Model;
using CMS.Notification;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Permissions;

namespace CMS.Management
{
    public partial class Configuration : CPanel
    {
        public static string key = "b14ca5898a4e4133bbce2ea2315a1916";

        [PrincipalPermission(SecurityAction.Demand, Role = "Super Administrator")]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            DataTable dt = M_Config.SelectAll();
            foreach(DataRow row in dt.Rows)
            {
                string password = Crypto.Decrypt(row["smtp_password"].ToString(), key);
                txtTelegram.Text = row["telegram_api"].ToString();
                txtSMTPMail.Text = row["smtp_mail"].ToString();
                txtSMTPPassword.Text = password;
                txtSMTPServer.Text = row["smtp_server"].ToString();
                txtSMTPPort.Text = row["smtp_port"].ToString();
            }
        }

        protected void cPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {            
            switch (e.Parameter.ToUpper())
            {
                case "SAVE":
                    M_Config config = new M_Config()
                    {
                        company_id = M_User.getCompanyId(),
                        telegram_api = txtTelegram.Text.Trim(),
                        smtp_mail = txtSMTPMail.Text.Trim(),
                        smtp_password = Crypto.Encrypt(txtSMTPPassword.Text.Trim(), key),
                        smtp_server = txtSMTPServer.Text.Trim(),
                        smtp_port = txtSMTPPort.Text.Trim().ToInteger()
                    };

                    try
                    {
                        using (var sqlConnection = new OracleConnection(Database.getConnectionString("Default")))
                        {
                            sqlConnection.Open();
                            using (OracleTransaction sqlTransaction = sqlConnection.BeginTransaction())
                            {
                                M_Config.InsertUpdate(config, sqlConnection, sqlTransaction);
                                sqlTransaction.Commit();
                                cPanel.alertSuccess("Data successfully updated");
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        cPanel.alertError(ex.Message.ToString());
                    }
                    
                    break;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/cpanel");
        }
    }
}