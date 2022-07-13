using Newtonsoft.Json.Linq;
using CMS.Helper;
using CMS.Management.Model;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace CMS.Management.Structure.Model
{
    public class M_Config
    {
        public int company_id { get; set; }
        public string telegram_api { get; set; }
        public string smtp_mail { get; set; }
        public string smtp_password { get; set; }
        public string smtp_server { get; set; }
        public int smtp_port { get; set; }

        public M_Config()
        {
            DataTable dt = M_Config.SelectAll();
            if(dt.Rows.Count > 0)
            {
                this.company_id = dt.Rows[0]["company_id"].ToInteger();
                this.telegram_api = dt.Rows[0]["telegram_api"].ToString();
                this.smtp_mail = dt.Rows[0]["smtp_mail"].ToString();
                this.smtp_password = dt.Rows[0]["smtp_password"].ToString();
                this.smtp_server = dt.Rows[0]["smtp_server"].ToString();
                this.smtp_port = dt.Rows[0]["smtp_port"].ToInteger();
            }
            
        }

        public static M_Config InsertUpdate(M_Config data, OracleConnection connection, OracleTransaction transaction)
        {
            string sql = string.Format(@"
                MERGE INTO m_configuration T
                USING (SELECT 
                    :company_id company_id,
                    ':telegram_api' telegram_api,
                    ':smtp_mail' smtp_mail,
                    ':smtp_password' smtp_password,
                    ':smtp_server' smtp_server,
                    :smtp_port smtp_port
                    FROM DUAL
                ) S
                ON (T.company_id=S.company_id)
                
                WHEN MATCHED THEN UPDATE
                SET                     
                    telegram_api=:telegram_api, 
                    smtp_mail=:smtp_mail, 
                    smtp_password=:smtp_password, 
                    smtp_server=:smtp_server, 
                    smtp_port=:smtp_port,
                    updateby=:updateby, 
                    updatedate=SYSDATE
                
                WHEN NOT MATCHED THEN
                INSERT(
                    company_id, 
                    telegram_api, 
                    smtp_mail, 
                    smtp_password,
                    smtp_server, 
                    smtp_port, 
                    createby, 
                    createdate
                )
                VALUES(
                    :company_id, 
                    :telegram_api, 
                    :smtp_mail, 
                    :smtp_password,
                    :smtp_server, 
                    :smtp_port, 
                    :createby, 
                    SYSDATE
                )");


            Database.querySQL(sql,
                connection, transaction,
                out OracleParameter[] param_out,
                new OracleParameter(":company_id", data.company_id.ToString()),
                new OracleParameter(":telegram_api", data.telegram_api.ToString()),
                new OracleParameter(":smtp_mail", data.smtp_mail.ToString()),
                new OracleParameter(":smtp_password", data.smtp_password.ToString()),
                new OracleParameter(":smtp_server", data.smtp_server.ToString()),
                new OracleParameter(":smtp_port", data.smtp_port.ToString()),
                new OracleParameter(":createby", M_User.getUserId().ToString()),
                new OracleParameter(":updateby", M_User.getUserId().ToString())
                );

            JObject json = param_out.toJObject();
            Log.Insert(Log.LogType.UPDATE, "Update data application configuration", json, connection, transaction);

            return data;
        }

        public static DataTable SelectAll()
        {
            DataTable dt = new DataTable();
            string sql = string.Format(@"SELECT * FROM m_configuration WHERE company_id=:company_id");

            var user = (M_User)HttpContext.Current.Session["M_User"];
            if(user == null)
            {
                dt = Database.getDataTable(sql, new OracleParameter(":company_id", M_User.getCompanyId()));
            }
            else
            {
                dt = Database.getDataTable(sql, new OracleParameter(":company_id", user.company_id));
            }
            
            return dt;
        }

        public static string GetTelegramAPI()
        {
            DataTable dt = M_Config.SelectAll();
            return dt.Rows[0]["telegram_api"].ToString();
        }

        public static string GetSMTPMail()
        {
            DataTable dt = M_Config.SelectAll();
            return dt.Rows[0]["smtp_mail"].ToString();
        }

        public static string GetSMTPPassword()
        {
            DataTable dt = M_Config.SelectAll();
            return dt.Rows[0]["smtp_password"].ToString();
        }

        public static string GetSMTPServer()
        {
            DataTable dt = M_Config.SelectAll();
            return dt.Rows[0]["smtp_server"].ToString();
        }

        public static string GetSMTPPort()
        {
            DataTable dt = M_Config.SelectAll();
            return dt.Rows[0]["smtp_port"].ToString();
        }
    }
}