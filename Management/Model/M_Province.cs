using Newtonsoft.Json.Linq;
using CMS.Helper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace CMS.Management.Model
{
    public class M_Province
    {
        public int province_code { get; set; }
        public string province_name { get; set; }
        public string country_code { get; set; }

        public static M_Province Insert(M_Province data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                INSERT INTO m_province 
                (province_code,province_name,country_code,createby,createdate)
                VALUES
                (:province_code,:province_name,:country_code,:createby,SYSDATE)", 
                connection, transaction,
                new OracleParameter(":province_code", data.province_code),
                new OracleParameter(":province_name", data.province_name),
                new OracleParameter(":country_code", data.country_code),
                new OracleParameter(":createby", M_User.getUserId())
                );

            return data;
        }

        public static M_Province Update(M_Province data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_province 
                SET 
                province_name=:province_name, 
                country_code=:country_code,                
                updateby=:updateby,
                updatedate=SYSDATE 
                WHERE province_code=:province_code;",
                connection, transaction,
                new OracleParameter(":province_code", data.province_code),
                new OracleParameter(":province_name", data.province_name),
                new OracleParameter(":country_code", data.country_code),                
                new OracleParameter(":updateby", M_User.getUserId())
                );

            return data;
        }

        public static M_Province Delete(M_Province data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_province 
                SET 
                isdelete=:isdelete, 
                deletedate=SYSDATE,
                deleteby=:deleteby
                WHERE province_code=:province_code;",
                connection, transaction,
                new OracleParameter(":isdelete", true),
                new OracleParameter(":province_code", data.province_code),
                new OracleParameter(":deleteby", M_User.getUserId())
                );

            return data;
        }

        public static M_Province Disable(M_Province data, OracleConnection connection, OracleTransaction transaction)
        {
            string sql = string.Format(@"UPDATE m_province
                SET 
                isactive=:isactive, 
                updatedate=SYSDATE,
                updateby=:updateby
                WHERE province_code=:province_code");

            Database.querySQL(sql, connection, transaction,
                new OracleParameter(":isactive", false),
                new OracleParameter(":province_code", data.province_code),
                new OracleParameter(":updateby", M_User.getUserId())
                );

            return data;
        }

        public static M_Province Enable(M_Province data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_province 
                SET isactive=:isactive, 
                updatedate=SYSDATE,
                updateby=:updateby
                WHERE province_code=:province_code;",
                connection, transaction,
                out OracleParameter[] param_out,
                new OracleParameter(":isactive", true),
                new OracleParameter(":province_code", data.province_code),
                new OracleParameter(":updateby", M_User.getUserId())
                );

            return data;
        }

        public static bool IsExist(M_Province data)
        {
            DataTable dt = Database.getDataTable("SELECT * FROM m_province  where province_name=:province_name",
                new OracleParameter(":province_name", data.province_name));

            if (dt.Rows.Count > 0) return true;

            return false;
        }

        public static DataTable SelectAll()
        {
            
            string sql = String.Format(@"SELECT * FROM m_province  WHERE isdelete=0", M_User.getDatabaseName());
            DataTable dt = Database.getDataTable(sql);
            return dt;
        }
    }

}