using CMS.Helper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace CMS.Management.Model
{
    public class M_Theme
    {
        public int theme_id { get; set; }
        public string theme_name { get; set; }
        public string theme_desc { get; set; }
        public string theme_location { get; set; }

        public bool isactive { get; set; }
        public bool isdelete { get; set; }
        public int createby { get; set; }
        public int updateby { get; set; }
        public int deleteby { get; set; }
        public string createdate { get; set; }
        public string updatedate { get; set; }
        public string deletedate { get; set; }

        public static M_Theme Insert(M_Theme data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                INSERT INTO m_theme (theme_name, theme_desc, theme_location, createby, createdate)
                VALUES (:theme_name, :theme_desc, :theme_location,:createby,SYSDATE)", connection, transaction,
                new OracleParameter(":theme_name", data.theme_name),
                new OracleParameter(":theme_desc", data.theme_desc),
                new OracleParameter(":theme_location", data.theme_location),
                new OracleParameter(":createby", M_User.getUserId())
                );

            return data;
        }

        public static M_Theme Update(M_Theme data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_theme 
                SET 
                theme_name=:theme_name, 
                theme_desc=:theme_desc,
                theme_location=:theme_location,
                updateby=:updateby,
                updatedate=SYSDATE 
                WHERE theme_id=@theme_id;",
                connection, transaction,
                new OracleParameter("@theme_id", data.theme_id),
                new OracleParameter(":theme_name", data.theme_name),
                new OracleParameter(":theme_desc", data.theme_desc),
                new OracleParameter(":theme_location", data.theme_location),
                new OracleParameter(":updateby", M_User.getUserId())
                );

            return data;
        }

        public static M_Theme Delete(M_Theme data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_theme 
                SET 
                isdelete=:isdelete, 
                deletedate=SYSDATE,
                deleteby=:deleteby
                WHERE theme_id=@theme_id;",
                connection, transaction,
                new OracleParameter(":isdelete", true),
                new OracleParameter("@theme_id", data.theme_id),
                new OracleParameter(":deleteby", M_User.getUserId())
                );
            return data;
        }

        public static M_Theme Disable(M_Theme data, OracleConnection connection, OracleTransaction transaction)
        {
            string sql = string.Format(@"UPDATE m_theme
                SET 
                isactive=:isactive, 
                updatedate=SYSDATE,
                updateby=:updateby
                WHERE theme_id=@theme_id");

            Database.querySQL(sql, connection, transaction,
                new OracleParameter(":isactive", false),
                new OracleParameter("@theme_id", data.theme_id),
                new OracleParameter(":updateby", M_User.getUserId())
                );
            return data;
        }

        public static M_Theme Enable(M_Theme data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_theme 
                SET isactive=:isactive, 
                updatedate=SYSDATE,
                updateby=:updateby
                WHERE theme_id=@theme_id;",
                connection, transaction,
                new OracleParameter(":isactive", true),
                new OracleParameter("@theme_id", data.theme_id),
                new OracleParameter(":updateby", M_User.getUserId())
                );
            return data;
        }

        public static bool IsExist(M_Theme data)
        {
            DataTable dt = Database.getDataTable("SELECT * FROM m_theme  where theme_name=:theme_name",
                new OracleParameter(":theme_name", data.theme_name));

            if (dt.Rows.Count > 0) return true;

            return false;
        }

        public static DataTable SelectAll()
        {
            
            string sql = String.Format(@"SELECT * FROM m_theme  WHERE isdelete=0", M_User.getDatabaseName());
            DataTable dt = Database.getDataTable(sql);

            List<M_Theme> data = new List<M_Theme>();
            foreach (DataRow row in dt.Rows)
            {
                data.Add(
                    new M_Theme()
                    {
                        theme_id = row["theme_id"].ToInteger(),
                        theme_name = row["theme_name"].ToString(),
                        theme_desc = row["theme_desc"].ToString(),
                        theme_location = row["theme_location"].ToString(),
                        isactive = row["isactive"].ToBoolean(),
                        isdelete = row["isdelete"].ToBoolean(),
                        createby = row["createby"].ToInteger(),
                        updateby = row["updateby"].ToInteger(),
                        deleteby = row["deleteby"].ToInteger(),
                        createdate = row["createdate"].ToString(),
                        updatedate = row["updatedate"].ToString(),
                        deletedate = row["deletedate"].ToString(),
                    }
                );
            }

            DataTable mdt = data.ToDataTable();
            return mdt;
        }
    }
}