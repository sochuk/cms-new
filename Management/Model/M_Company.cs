using CMS.Helper;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace CMS.Management.Model
{
    public class M_Company_Core
    {
        public int company_id { get; set; }
        public string company_name { get; set; }
        public string company_desc { get; set; }

        public bool isactive { get; set; }
        public bool isdelete { get; set; }
        public int createby { get; set; }
        public int updateby { get; set; }
        public int deleteby { get; set; }
        public string createdate { get; set; }
        public string updatedate { get; set; }
        public string deletedate { get; set; }

        public static M_Company_Core Insert(M_Company_Core data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                INSERT INTO m_company(company_name, company_desc, createby, createdate)
                VALUES(:company_name, :company_desc, :createby, SYSDATE)", connection, transaction,
                new OracleParameter(":company_name", data.company_name),
                new OracleParameter(":company_desc", data.company_desc),
                new OracleParameter(":createby", M_User.getUserId())
                ); 

            return data;
        }

        public static M_Company_Core Update(M_Company_Core data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_company 
                SET 
                company_name=:company_name, 
                company_desc=:company_desc,
                updateby=:updateby,
                updatedate=SYSDATE
                WHERE company_id=:company_id;",
                connection, transaction,
                new OracleParameter(":company_id", data.company_id),
                new OracleParameter(":company_name", data.company_name),
                new OracleParameter(":company_desc", data.company_desc),
                new OracleParameter(":updateby", M_User.getUserId())
                );

            return data;
        }

        public static M_Company_Core Delete(M_Company_Core data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_company 
                SET 
                isdelete=:isdelete, 
                deletedate=SYSDATE,
                deleteby=:deleteby
                WHERE company_id=:company_id;",
                connection, transaction,
                new OracleParameter(":isdelete", true),
                new OracleParameter(":company_id", data.company_id),
                new OracleParameter(":deleteby", M_User.getUserId())
                );
            return data;
        }

        public static M_Company_Core Disable(M_Company_Core data, OracleConnection connection, OracleTransaction transaction)
        {
            string sql = string.Format(@"UPDATE m_company
                SET 
                isactive=:isactive, 
                updatedate=SYSDATE,
                updateby=:updateby
                WHERE company_id=:company_id");

            Database.querySQL(sql, connection, transaction,
                new OracleParameter(":isactive", false),
                new OracleParameter(":company_id", data.company_id),
                new OracleParameter(":updateby", M_User.getUserId())
                );
            return data;
        }

        public static M_Company_Core Enable(M_Company_Core data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_company 
                SET 
                isactive=:isactive, 
                updatedate=SYSDATE,
                updateby=:updateby
                WHERE company_id=:company_id;",
                connection, transaction,
                new OracleParameter(":isactive", true),
                new OracleParameter(":company_id", data.company_id),
                new OracleParameter(":updateby", M_User.getUserId())
                );
            return data;
        }

        public static bool IsExist(M_Company_Core data)
        {
            DataTable dt = Database.getDataTable("SELECT * FROM m_company  where company_name=:company_name",
                new OracleParameter(":company_name", data.company_name));

            if (dt.Rows.Count > 0) return true;

            return false;
        }

        public static DataTable SelectAll()
        {
            
            string sql = string.Format(@"SELECT * FROM m_company  WHERE isdelete=0", M_User.getDatabaseName());
            DataTable dt = Database.getDataTable(sql);

            List<M_Company_Core> data = new List<M_Company_Core>();
            foreach (DataRow row in dt.Rows)
            {
                data.Add(
                    new M_Company_Core()
                    {
                        company_id = row["company_id"].ToInteger(),
                        company_name = row["company_name"].ToString(),
                        company_desc = row["company_desc"].ToString(),
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