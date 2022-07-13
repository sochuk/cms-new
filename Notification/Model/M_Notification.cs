using Microsoft.AspNet.SignalR;
using CMS.Helper;
using CMS.Hubs;
using CMS.Management.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace CMS.Notification.Model
{
    public enum NotificationType
    {
        DEFAULT,
        ADD,
        EDIT,
        DELETE,
        REQUEST,
        APPROVED,
        REJECT,
        RECEIVED,
        OPEN,
        CLOSED,
    }

    public class M_Notification
    {
        public int notif_id { get; set; }
        public int to_user_id { get; set; }
        public int from_user_id { get; set; }
        public string notif_title { get; set; }
        public string notif_message { get; set; }
        public NotificationType notif_type { get; set; }
        public string url { get; set; }
        
        private string user_to_username { get; set; }

        public static M_Notification Insert(M_Notification data, OracleConnection sqlConnection, OracleTransaction sqlTransaction)
        {
            DataTable dt = Database.queryScalar(@"
                INSERT INTO notification(notif_title, notif_message, notif_type, url, from_user_id, to_user_id, createdate)
                VALUES(:notif_title, :notif_message, :notif_type, :url, :from_user_id, :to_user_id, SYSDATE);
                SELECT @@IDENTITY AS 'id';", sqlConnection, sqlTransaction,
                new OracleParameter(":notif_title", data.notif_title),
                new OracleParameter(":notif_message", data.notif_message),
                new OracleParameter(":notif_type", data.notif_type),
                new OracleParameter(":url", data.url),
                new OracleParameter(":from_user_id", (data.from_user_id == 0 ? M_User.getUserId() : data.from_user_id)),
                new OracleParameter(":to_user_id", data.to_user_id)
                );

            data.from_user_id = (data.from_user_id == 0 ? M_User.getUserId() : data.from_user_id);
            data.notif_id = dt.Rows[0]["id"].ToString().ToInteger();
            return data;
        }       
        
        public static DataTable SelectAll(int Limit=0)
        {
            string sql = string.Format(@"
                    SELECT {0} N.*, U.fullname, U.username, U.photo   
                    FROM notification  N
                    LEFT JOIN m_user  U ON N.from_user_id=U.user_id
                    WHERE to_user_id=:to_user_id
                    ORDER BY N.createdate DESC",
                    (Limit > 0) ? "TOP " + Limit : "");

            DataTable dt = Database.getDataTable(sql, 
                new OracleParameter(":to_user_id", M_User.getUserId()));
            return dt;
        }

        public static DataTable SelectOffset(int offset, int limit)
        {
            string sql = string.Format(@"
                    SELECT N.*, U.fullname, U.username, U.photo   
                    FROM notification  N
                    LEFT JOIN m_user  U ON N.from_user_id=U.user_id
                    WHERE to_user_id=:to_user_id
                    ORDER BY N.createdate DESC
                    OFFSET {0} ROWS
                    FETCH FIRST {1} ROWS ONLY;", 
                    offset, limit);

            DataTable dt = Database.getDataTable(sql,
                new OracleParameter(":to_user_id", M_User.getUserId()));
            return dt;
        }

        public static DataTable SelectTop(int count, int offset)
        {
            DataTable dt = M_Notification.SelectAll(10);
            return dt;
        }

        public static int getNotification_Unread()
        {
            string sql = @"
                SELECT COUNT(*) unread FROM notification  WHERE to_user_id=:to_user_id AND isread=0";

            DataTable dt = Database.queryScalar(sql,
                new OracleParameter(":to_user_id", M_User.getUserId())
                );

            return dt.Rows[0]["unread"].ToString().ToInteger();
        }
        
        public static void setNotificationRead(int notif_id, bool all)
        {
            if (all)
            {
                Database.querySQL(@"
                UPDATE notification SET isread=1 WHERE to_user_id=:to_user_id",
                new OracleParameter(":to_user_id", M_User.getUserId())
                );
            }
            else
            {
                Database.querySQL(@"
                UPDATE notification SET isread=1 WHERE notif_id=@notif_id ",
                new OracleParameter("@notif_id", notif_id)
                );
            }            
        }
    }
}