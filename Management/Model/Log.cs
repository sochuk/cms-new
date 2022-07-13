using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CMS.Helper;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace CMS.Management.Model
{
    public class Log
    {        
        public string log_title { get; set; }
        public string log_content { get; set; }
        public LogType log_type { get; set; }
        private int user_id { get; set; }
        private string local_ip { get; set; }
        private string remote_ip { get; set; }
        private string location { get; set; }
        private string browser { get; set; }
        private string module_id { get; set; }

        public enum LogType
        {
            [Description("Add data")]
            ADD,
            [Description("Update data")]
            UPDATE,
            [Description("Delete data")]
            DELETE,
            [Description("Enable data")]
            ENABLE,
            [Description("Disable data")]
            DISABLE,
            [Description("View data")]
            VIEW,
            [Description("Login")]
            LOGIN,
            [Description("Logout")]
            LOGOUT,
            [Description("Expired")]
            EXPIRED,
            [Description("Approved")]
            APPROVE,
            [Description("Rejected")]
            REJECT
        }

        public static void Insert(LogType log_type, string log_title, object log_content, OracleConnection sqlConnection, OracleTransaction sqlTransaction)
        {
            if (!CPanel.Logging) return;
            Database.querySQL(@"
                INSERT INTO log_user(
                        log_date,
                        log_type,
                        log_title,
                        log_content,
                        user_id,
                        local_ip,
                        remote_ip,
                        location,
                        browser,
                        module_id
                    )
                    VALUES(
                        SYSDATE,
                        :log_type,
                        :log_title,
                        :log_content,
                        :user_id,
                        :local_ip,
                        :remote_ip,
                        :location,
                        :browser,
                        :module_id
                 )",
                sqlConnection, sqlTransaction,
                    new OracleParameter(":log_type", (int)log_type),
                    new OracleParameter(":log_title", log_title),
                    new OracleParameter(":log_content", JsonConvert.SerializeObject(log_content, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore })),
                    new OracleParameter(":local_ip", M_User.getLocalIP() == string.Empty ? M_User.getSessionLocalIPAddress() : M_User.getLocalIP()),
                    new OracleParameter(":remote_ip", M_User.getRemoteIP() == string.Empty ? M_User.getSessionRemoteIPAddress() : M_User.getRemoteIP()),
                    new OracleParameter(":location", M_User.getLocation() == string.Empty ? M_User.getSessionLocation() : M_User.getLocation()),
                    new OracleParameter(":browser", HttpContext.Current.Request.Browser.Browser),
                    new OracleParameter(":module_id", Application.GetModuleId()),
                    new OracleParameter(":user_id", M_User.getUserId())
                );
        }

        public static void Insert(LogType log_type, string log_title, object log_content)
        {
            if (!CPanel.Logging) return;
            using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
            {
                cnn.Open();
                using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                {
                    Database.querySQL(@"
                    INSERT INTO log_user(
                        log_date,
                        log_type,
                        log_title,
                        log_content,
                        user_id,
                        local_ip,
                        remote_ip,
                        location,
                        browser,
                        module_id
                    )
                    VALUES(
                        SYSDATE,
                        :log_type,
                        :log_title,
                        :log_content,
                        :user_id,
                        :local_ip,
                        :remote_ip,
                        :location,
                        :browser,
                        :module_id
                    )",
                    cnn, sqlTransaction,
                    new OracleParameter(":log_type", log_type),
                    new OracleParameter(":log_title", log_title),
                    new OracleParameter(":log_content", JsonConvert.SerializeObject(log_content, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore })),
                    new OracleParameter(":local_ip", M_User.getLocalIP()),
                    new OracleParameter(":remote_ip", M_User.getRemoteIP()),
                    new OracleParameter(":location", M_User.getLocation()),
                    new OracleParameter(":browser", HttpContext.Current.Request.Browser.Browser),
                    new OracleParameter(":module_id", Application.GetModuleId()),
                    new OracleParameter(":user_id", HttpContext.Current.User.Identity.Get_Id())
                    );

                    sqlTransaction.Commit();
                }
            }
        }

        public static void Insert(LogType log_type, string log_title, object log_content, int user_id, string local_ip, string remote_ip, string location)
        {
            if (!CPanel.Logging) return;
            using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
            {
                cnn.Open();
                using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                {
                    Database.querySQL(@"
                    INSERT INTO log_user(
                        log_date,
                        log_type,
                        log_title,
                        log_content,
                        user_id,
                        local_ip,
                        remote_ip,
                        location,
                        browser,
                        module_id
                    )
                    VALUES(
                        SYSDATE,
                        :log_type,
                        :log_title,
                        :log_content,
                        :user_id,
                        :local_ip,
                        :remote_ip,
                        :location,
                        :browser,
                        :module_id
                    )",
                    cnn, sqlTransaction,
                    new OracleParameter(":log_type", (int)log_type),
                    new OracleParameter(":log_title", log_title),
                    new OracleParameter(":log_content", JsonConvert.SerializeObject(log_content, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore })),
                    new OracleParameter(":local_ip", local_ip),
                    new OracleParameter(":remote_ip", remote_ip),
                    new OracleParameter(":location", location),
                    new OracleParameter(":browser", HttpContext.Current.Request.Browser.Browser),
                    new OracleParameter(":module_id", Application.GetModuleId()),
                    new OracleParameter(":user_id", HttpContext.Current.User.Identity.Get_Id().ToInteger() == 0 ? user_id : HttpContext.Current.User.Identity.Get_Id().ToInteger())
                    );

                    sqlTransaction.Commit();
                }
            }
        }

        /// <summary>
        /// Get all activity
        /// </summary>
        /// <param name="Limit">Limit selection</param>
        /// <returns></returns>
        public static DataTable SelectAll()
        {
            string sql = @"SELECT * FROM log_user ORDER BY log_id DESC";
            DataTable dt = Database.getDataTable(sql);
            return dt;
        }

        public static DataTable SelectAll(int module_id)
        {
            string sql = string.Format(@"SELECT 
               log_id
              ,log_date
              ,log_type
              ,user_id
              ,local_ip
              ,remote_ip
              ,location
              FROM log_user WHERE module_id=:module_id ORDER BY log_date DESC");
            DataTable dt = Database.getDataTable(sql, new OracleParameter(":module_id", module_id)).ToColumnLowerCase();
            return dt;
        }

        public static DataTable GetDetail<T>(int log_id)
        {
            string sql = string.Format(@"SELECT 
               log_title
              ,log_content
              FROM log_user  WHERE log_id=:log_id");
            DataTable dt = Database.getDataTable(sql, new OracleParameter(":log_id", log_id));
            dt.Columns["log_title"].ColumnName = "Title";
            dt.Columns["log_content"].ColumnName = "Data";

            return dt;
        }

        /// <summary>
        /// Get direct employee activity
        /// </summary>
        /// <param name="Limit">Limit selection</param>
        /// <returns></returns>
        public static DataTable SelectDirectEmployeeActivity(int Limit = 0)
        {
            string sql = string.Format(@"
                SELECT {0} L.*, U.fullname, U.photo FROM log_user  L
                LEFT JOIN m_user  U ON L.user_id=U.user_id
                WHERE L.user_id IN {1}
                ORDER BY L.log_date DESC
                ",
                (Limit > 0 ? "TOP " + Limit : ""), M_User.SQLHelper.getINSqlDirectEmployee());

            DataTable dt = Database.getDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["photo"].ToString() == null || row["photo"].ToString() == string.Empty)
                    {
                        row["photo"] = Application.GetPath() + "/Content/images/unknown.jpg";
                    }
                    else
                    {
                        if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(dt.Rows[0]["photo"].ToString())))
                        {
                            dt.Rows[0]["photo"] = Application.GetPath() + "/Content/images/unknown.jpg";
                        }
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// Get selected employee activity
        /// </summary>
        /// <param name="Limit">Limit selection</param>
        /// <returns></returns>
        public static DataTable SelectEmployeeActivity(int user_id, int Limit = 0)
        {
            string sql = string.Format(@"
                SELECT {0} L.*, U.fullname, U.photo FROM log_user  L
                LEFT JOIN m_user  U ON L.user_id=U.user_id
                WHERE L.user_id=:user_id
                ORDER BY L.log_date DESC
                ",
                (Limit > 0 ? "TOP " + Limit : ""), M_User.SQLHelper.getINSqlDirectEmployee());

            DataTable dt = Database.getDataTable(sql, new OracleParameter(":user_id", user_id));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["photo"].ToString() == null || row["photo"].ToString() == string.Empty)
                    {
                        row["photo"] = Application.GetPath() + "/Content/images/unknown.jpg";
                    }
                    else
                    {
                        if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(dt.Rows[0]["photo"].ToString())))
                        {
                            dt.Rows[0]["photo"] = Application.GetPath() + "/Content/images/unknown.jpg";
                        }
                    }
                }
            }
            return dt;
        }
    }

}