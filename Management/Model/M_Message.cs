using CMS.Helper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMS.Management.Model
{
    public class M_Message
    {
        public int user_from { get; set; }
        public int user_to { get; set; }
        public int message_content { get; set; }

        public static void Insert(int to_user_id, string message)
        {
            string sql = @"
                INSERT INTO message(user_from, user_to, message_content) 
                VALUES(:user_from, :user_to, :message_content)";

            Database.querySQL(sql, 
                new OracleParameter(":user_from", M_User.getUserId()),
                new OracleParameter(":user_to", to_user_id),
                new OracleParameter(":message_content", message)
                );
        }

        public static DataTable getMessage_Data(int user_from)
        {
            string sql = @"
              UPDATE message SET isread=1 WHERE user_to=:user_to AND user_from=:user_from;
              SELECT A.*, B.username,B.fullname FROM
              (SELECT user_from, message_content, createdate FROM message  WHERE (user_to=:user_to AND user_from=:user_from) 
              OR (user_to=:user_from AND user_from=:user_to)) A
              LEFT JOIN m_user B  ON A.user_from=B.user_id
              ORDER BY A.createdate ASC;";

            DataTable dt = Database.getDataTable(sql,
                new OracleParameter(":user_to", M_User.getUserId()),
                new OracleParameter(":user_from", user_from)
                );

            return dt;
        }

        public static int getMessage_Unread()
        {
            string sql = @"
                SELECT COUNT(*) unread FROM message  WHERE user_to=:user_to AND isread=0";

            DataTable dt = Database.queryScalar(sql,
                new OracleParameter(":user_to", M_User.getUserId())
                );

            return dt.Rows[0]["unread"].ToString().ToInteger();
        }

        public static DataTable getMessage_UnreadUserData()
        {
            string sql = @"
                SELECT A.*,B.username FROM 
                (SELECT user_from, COUNT(user_from) unread FROM message  WHERE user_to=:user_to AND isread=0 GROUP BY user_from) A
                LEFT JOIN m_user B  ON A.user_from=B.user_id";

            DataTable dt = Database.getDataTable(sql,
                new OracleParameter(":user_to", M_User.getUserId())
                );

            return dt;
        }
    }
}