using Newtonsoft.Json.Linq;
using CMS.Helper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;

namespace CMS.Management.Model
{
    public class M_Role
    {
        public int role_id { get; set; }
        public string role_name { get; set; }
        public string role_desc { get; set; }
        public bool allow_enabledisable { get; set; }
        public bool allow_create { get; set; }
        public bool allow_update { get; set; }
        public bool allow_delete { get; set; }
        public bool allow_export { get; set; }
        public bool allow_import { get; set; }

        public bool isactive { get; set; }
        public bool isdelete { get; set; }
        public int createby { get; set; }
        public int updateby { get; set; }
        public int deleteby { get; set; }
        public string createdate { get; set; }
        public string updatedate { get; set; }
        public string deletedate { get; set; }

        public static M_Role Insert(M_Role data, OracleConnection connection, OracleTransaction transaction)
        {
            DataTable dt = Database.queryScalar(@"
                BEGIN
                INSERT INTO m_role 
                (role_name,role_desc,allow_create,allow_update,allow_delete,allow_export,allow_import,allow_enabledisable,createdate, createby)
                VALUES
                (:role_name,:role_desc,:allow_create,:allow_update,:allow_delete,:allow_export,:allow_import,:allow_enabledisable,SYSDATE,:createby);
                END;", 
                connection, transaction,
                new OracleParameter(":role_name", data.role_name),
                new OracleParameter(":role_desc", data.role_desc),
                new OracleParameter(":allow_create", data.allow_create.ToInteger().ToString()),
                new OracleParameter(":allow_update", data.allow_update.ToInteger().ToString()),
                new OracleParameter(":allow_delete", data.allow_delete.ToInteger().ToString()),
                new OracleParameter(":allow_export", data.allow_export.ToInteger().ToString()),
                new OracleParameter(":allow_import", data.allow_import.ToInteger().ToString()),
                new OracleParameter(":allow_enabledisable", data.allow_enabledisable.ToInteger().ToString()),
                new OracleParameter(":createby", M_User.getUserId().ToString())
                );

            return data;
        }

        public static M_Role Update(M_Role data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_role 
                SET 
                role_name=:role_name, 
                role_desc=:role_desc,
                allow_create=:allow_create,
                allow_update=:allow_update,
                allow_delete=:allow_delete,
                allow_export=:allow_export,
                allow_import=:allow_import,
                allow_enabledisable=:allow_enabledisable,
                updateby=:updateby,
                updatedate=SYSDATE 
                WHERE role_id=:role_id",
                connection, transaction,
                new OracleParameter(":role_id", data.role_id),
                new OracleParameter(":role_name", data.role_name),
                new OracleParameter(":role_desc", data.role_desc),
                new OracleParameter(":allow_create", data.allow_create.ToInteger().ToString()),
                new OracleParameter(":allow_update", data.allow_update.ToInteger().ToString()),
                new OracleParameter(":allow_delete", data.allow_delete.ToInteger().ToString()),
                new OracleParameter(":allow_export", data.allow_export.ToInteger().ToString()),
                new OracleParameter(":allow_import", data.allow_import.ToInteger().ToString()),
                new OracleParameter(":allow_enabledisable", data.allow_enabledisable.ToInteger().ToString()),
                new OracleParameter(":updateby", M_User.getUserId())
                );

            return data;
        }

        public static M_Role Delete(M_Role data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_role 
                SET 
                isdelete=:isdelete, 
                deletedate=SYSDATE,
                deleteby=:deleteby
                WHERE role_id=:role_id",
                connection, transaction,
                new OracleParameter(":isdelete", "1"),
                new OracleParameter(":role_id", data.role_id),
                new OracleParameter(":deleteby", M_User.getUserId())
                );

            return data;
        }

        public static M_Role Disable(M_Role data, OracleConnection connection, OracleTransaction transaction)
        {
            string sql = string.Format(@"UPDATE m_role
                SET 
                isactive=:isactive, 
                updatedate=SYSDATE,
                updateby=:updateby
                WHERE role_id=:role_id");

            Database.querySQL(sql, 
                connection, transaction,
                new OracleParameter(":isactive", "0"),
                new OracleParameter(":role_id", data.role_id),
                new OracleParameter(":updateby", M_User.getUserId())
                );

            return data;
        }

        public static M_Role Enable(M_Role data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_role 
                SET isactive=:isactive, 
                updatedate=SYSDATE,
                updateby=:updateby
                WHERE role_id=:role_id",
                connection, transaction,
                new OracleParameter(":isactive", "1"),
                new OracleParameter(":role_id", data.role_id),
                new OracleParameter(":updateby", M_User.getUserId())
                );

            return data;
        }

        public static bool IsExist(M_Role data)
        {
            DataTable dt = Database.getDataTable("SELECT * FROM m_role  where role_name=:role_name",
                new OracleParameter(":role_name", data.role_name));

            if (dt.Rows.Count > 0) return true;

            return false;
        }

        public static DataTable SelectAll()
        {
            
            string sql = String.Format(@"SELECT * FROM m_role  WHERE isdelete=0", M_User.getDatabaseName());
            DataTable dt = Database.getDataTable(sql);

            List<M_Role> role = new List<M_Role>();
            foreach (DataRow row in dt.Rows)
            {
                role.Add(
                    new M_Role()
                    {
                        role_id = row["role_id"].ToInteger(),
                        role_name= row["role_name"].ToString(),
                        role_desc= row["role_desc"].ToString(),
                        allow_enabledisable= row["allow_enabledisable"].ToBoolean(),
                        allow_create= row["allow_create"].ToBoolean(),
                        allow_update= row["allow_update"].ToBoolean(),
                        allow_delete= row["allow_delete"].ToBoolean(),
                        allow_export= row["allow_export"].ToBoolean(),
                        allow_import= row["allow_import"].ToBoolean(),
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

            DataTable mdt = role.ToDataTable();

            return mdt;
        }
    }

}