using CMS.Helper;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace CMS.Management.Model
{
    public class M_Group
    {
        public int group_id { get; set; }
        public string group_name { get; set; }
        public string group_desc { get; set; }
        public int role_id { get; set; }
        public string role_name { get; set; }

        public bool isactive { get; set; }
        public bool isdelete { get; set; }
        public int createby { get; set; }
        public int updateby { get; set; }
        public int deleteby { get; set; }
        public string createdate { get; set; }
        public string updatedate { get; set; }
        public string deletedate { get; set; }

        public static M_Group Insert(M_Group group, OracleConnection connection, OracleTransaction transaction)
        {
            DataTable dt = Database.queryScalar(@"
                INSERT INTO m_group(group_name, group_desc, role_id, createby, createdate)
                VALUES(:group_name, :group_desc, :role_id, :createby, SYSDATE)", connection, transaction,
                new OracleParameter(":group_name", group.group_name),
                new OracleParameter(":group_desc", group.group_desc),
                new OracleParameter(":role_id", group.role_id),
                new OracleParameter(":createby", M_User.getUserId())
                );

            return group;
        }

        public static M_Group Update(M_Group group, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_group 
                SET group_name=:group_name, 
                group_desc=:group_desc,
                role_id=:role_id,
                updateby=:updateby,
                updatedate=SYSDATE
                WHERE group_id=:group_id",
                connection, transaction,
                new OracleParameter(":group_id", group.group_id),
                new OracleParameter(":group_name", group.group_name),
                new OracleParameter(":group_desc", group.group_desc),
                new OracleParameter(":role_id", group.role_id),
                new OracleParameter(":updateby", M_User.getUserId())
                );

            return group;
        }

        public static M_Group Delete(M_Group data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_group 
                SET 
                isdelete=:isdelete, 
                deletedate=SYSDATE,
                deleteby=:deleteby
                WHERE group_id=:group_id",
                connection, transaction,
                new OracleParameter(":isdelete", true.ToInteger()),
                new OracleParameter(":group_id", data.group_id),
                new OracleParameter(":deleteby", M_User.getUserId())
                );
            return data;
        }

        public static M_Group Disable(M_Group data, OracleConnection connection, OracleTransaction transaction)
        {
            string sql = string.Format(@"UPDATE m_group
                SET 
                isactive=:isactive, 
                updatedate=SYSDATE,
                updateby=:updateby
                WHERE group_id=:group_id");

            Database.querySQL(sql, connection, transaction,
                new OracleParameter(":isactive", false.ToInteger()),
                new OracleParameter(":group_id", data.group_id),
                new OracleParameter(":updateby", M_User.getUserId())
                );
            return data;
        }

        public static M_Group Enable(M_Group data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_group 
                SET 
                isactive=:isactive, 
                updatedate=SYSDATE,
                updateby=:updateby
                WHERE group_id=:group_id",
                connection, transaction,
                new OracleParameter(":isactive", true.ToInteger()),
                new OracleParameter(":group_id", data.group_id),
                new OracleParameter(":updateby", M_User.getUserId())
                );
            return data;
        }

        public static bool IsExist(M_Group data)
        {
            DataTable dt = Database.getDataTable("SELECT * FROM m_group  where group_name=:group_name",
                new OracleParameter(":group_name", data.group_name));

            if (dt.Rows.Count > 0) return true;

            return false;
        }

        public static DataTable SelectAll()
        {
            string sql = string.Format(@"
                    SELECT G.*,R.role_name 
                    FROM m_group  G
                    LEFT JOIN m_role  R ON G.role_id=R.role_id
                    WHERE G.isdelete=0", M_User.getDatabaseName());
            DataTable dt = Database.getDataTable(sql);

            List<M_Group> data = new List<M_Group>();
            foreach (DataRow row in dt.Rows)
            {
                data.Add(
                    new M_Group()
                    {
                        group_id = row["group_id"].ToInteger(),
                        group_name = row["group_name"].ToString(),
                        group_desc = row["group_desc"].ToString(),
                        role_id = row["role_id"].ToInteger(),
                        role_name = row["role_name"].ToString(),
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