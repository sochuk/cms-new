using CMS.Helper;
using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace CMS.Management.Model
{
    public class M_City
    {
        public int city_id { get; set; }
        public string city_name { get; set; }
        public int province_code { get; set; }

        public static M_City Insert(M_City data, OracleConnection connection, OracleTransaction transaction)
        {
            
            Database.querySQL(@"
                INSERT INTO m_city 
                (province_code,city_name,createby,createdate)
                VALUES
                (:province_code,:city_name,:createby,SYSDATE)", 
                connection, transaction,
                new OracleParameter(":city_name", data.city_name),
                new OracleParameter(":province_code", data.province_code),
                new OracleParameter(":createby", M_User.getUserId())
                );

            return data;
        }

        public static M_City Update(M_City data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_city 
                SET 
                city_name=:city_name,
                province_code=:province_code,
                updateby=:updateby,
                updatedate=SYSDATE 
                WHERE city_id=:city_id;",
                connection, transaction,
                new OracleParameter(":city_name", data.city_name),
                new OracleParameter(":province_code", data.province_code),
                new OracleParameter(":city_id", data.city_id),                
                new OracleParameter(":updateby", M_User.getUserId())
                );

            return data;
        }

        public static M_City Delete(M_City data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_city 
                SET 
                isdelete=:isdelete, 
                deletedate=SYSDATE,
                deleteby=:deleteby
                WHERE city_id=:city_id;",
                connection, transaction,
                new OracleParameter(":isdelete", true),
                new OracleParameter(":city_id", data.city_id),
                new OracleParameter(":deleteby", M_User.getUserId())
                );

            return data;
        }

        public static M_City Disable(M_City data, OracleConnection connection, OracleTransaction transaction)
        {
            string sql = string.Format(@"UPDATE m_city
                SET 
                isactive=:isactive, 
                updatedate=SYSDATE,
                updateby=:updateby
                WHERE city_id=:city_id");

            Database.querySQL(sql, connection, transaction,
                new OracleParameter(":city_id", data.city_id),
                new OracleParameter(":isactive", false),
                new OracleParameter(":updateby", M_User.getUserId())
                );

            return data;
        }

        public static M_City Enable(M_City data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_city 
                SET isactive=:isactive, 
                updatedate=SYSDATE,
                updateby=:updateby
                WHERE province_code=:province_code;",
                connection, transaction,
                new OracleParameter(":isactive", true),
                new OracleParameter(":city_id", data.city_id),
                new OracleParameter(":updateby", M_User.getUserId())
                );

            return data;
        }

        public static bool IsExist(M_City data)
        {
            DataTable dt = Database.getDataTable("SELECT * FROM m_city  where city_name=:city_name",
                new OracleParameter(":city_name", data.city_name));

            if (dt.Rows.Count > 0) return true;

            return false;
        }

        public static DataTable SelectAll()
        {
            string sql = String.Format(@"SELECT * FROM m_city  WHERE isdelete=0", M_User.getDatabaseName());
            DataTable dt = Database.getDataTable(sql);
            return dt;
        }

        public static DataTable SelectAll(string province_code)
        {
            string sql = String.Format(@"SELECT * FROM m_city  WHERE isdelete=0 AND province_code=:province_code", M_User.getDatabaseName());
            DataTable dt = Database.getDataTable(sql, new OracleParameter(":province_code", province_code));
            return dt;
        }
    }

}