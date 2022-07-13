using Newtonsoft.Json.Linq;
using CMS.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace CMS.Management.Model
{
    public class M_Token
    {
        public int token_id { get; set; }
        public int user_id { get; set; }
        public string token { get; set; }

        public static string GenerateToken(int user_id, OracleConnection connection, OracleTransaction transaction)
        {
            string token = Crypto.RandomString(6, true).ToUpper();
            Database.querySQL(@"
                BEGIN
                UPDATE m_token SET isactive=0 WHERE user_id=:user_id;
                INSERT INTO m_token (user_id, token, isactive, createdate)
                VALUES(:user_id, :token, :isactive, SYSDATE);
                END;",
                connection, transaction,
                out OracleParameter[] param_out,
                new OracleParameter(":user_id", user_id),
                new OracleParameter(":token", token),
                new OracleParameter(":isactive", true.ToInteger())
                );

            JObject json = param_out.toJObject();
            Log.Insert(Log.LogType.UPDATE, "Generate new token", json, connection, transaction);

            return token;
        }

        public static DataTable SelectAll()
        {
            string sql = string.Format(@"
                SELECT * FROM m_configuration WHERE company_id=:company_id
                ", M_User.getDatabaseName());

            DataTable dt = Database.getDataTable(sql, new OracleParameter(":company_id", M_User.getCompanyId()));
            return dt;
        }

        public static bool ValidateToken(int user_id, string token)
        {
            string sql = string.Format(@"
                SELECT token FROM m_token 
                WHERE user_id=:user_id AND isactive=1 AND token=:token AND rownum=1
                ", M_User.getDatabaseName());

            DataTable dt = Database.getDataTable(sql, 
                new OracleParameter(":user_id", user_id),
                new OracleParameter(":token", token));

            if (dt.Rows.Count > 0) return true;
            return false;
        }
    }
}