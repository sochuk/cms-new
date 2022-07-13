using KMS.Helper;
using KMS.Management.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Oracle.ManagedDataAccess.Client;

namespace KMS.Management.Structure.Model
{
    public class M_Organization
    {
        public int organization_id { get; set; }
        public int division_id { get; set; }
        public int user_id { get; set; }
        public int user_root { get; set; }
        public bool can_approve { get; set; }
        public bool isactive { get; set; }
        public bool isdelete { get; set; }

        public static M_Organization Insert(M_Organization organization, OracleConnection connection, OracleTransaction transaction)
        {
            DataTable dt = Database.queryScalar(@"
                INSERT INTO m_organization
                (division_id, user_id, user_root,can_approve, createby, createdate)
                VALUES
                (@division_id, @user_id, @user_root, @can_approve, @createby, GETDATE());
                SELECT @@IDENTITY AS 'id';",
                connection, transaction,
                new OracleParameter("@division_id", organization.division_id),
                new OracleParameter("@user_id", organization.user_id),
                new OracleParameter("@user_root", organization.user_root),
                new OracleParameter("@can_approve", organization.can_approve),
                new OracleParameter("@createby", M_User.getUserId())
                );

            organization.organization_id = dt.Rows[0]["id"].ToString().ToInteger();
            return organization;
        }

        public static M_Organization Update(M_Organization organization, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_organization 
                SET division_id=@division_id,
                user_id=@user_id,
                updateby=@updateby,
                user_root=@user_root,
                can_approve=@can_approve,
                updatedate=GETDATE()
                WHERE organization_id=@organization_id;",
                connection, transaction,
                new OracleParameter("@organization_id", organization.organization_id),
                new OracleParameter("@division_id", organization.division_id),
                new OracleParameter("@user_id", organization.user_id),
                new OracleParameter("@user_root", organization.user_root),
                new OracleParameter("@can_approve", organization.can_approve),
                new OracleParameter("@updateby", M_User.getUserId())
                );

            return organization;
        }

        public static M_Organization InsertUpdate(M_Organization organization, OracleConnection connection, OracleTransaction transaction)
        {
            string sql = string.Format(@"
                MERGE INTO dbo.m_organization WITH(HOLDLOCK) AS T
                USING (SELECT 
                @division_id division_id,
                @user_id user_id, 
                @user_root user_root,
                @can_approve can_approve
                ) AS S
                (division_id, user_id, user_root, can_approve)
                ON (T.user_id=S.user_id)
                WHEN MATCHED THEN UPDATE
                SET division_id=@division_id, user_root=@user_root, can_approve=@can_approve, updateby=@updateby, updatedate=GETDATE()
                WHEN NOT MATCHED THEN
                INSERT(division_id, user_id, user_root, can_approve, createby, createdate)
                VALUES(@division_id, @user_id, @user_root, @can_approve, @createby, GETDATE());
                ");


            DataTable dt = Database.queryScalar(sql,
                connection, transaction,
                new OracleParameter("@organization_id", organization.organization_id),
                new OracleParameter("@division_id", organization.division_id),
                new OracleParameter("@user_id", organization.user_id),
                new OracleParameter("@user_root", organization.user_root),
                new OracleParameter("@can_approve", organization.can_approve),
                new OracleParameter("@createby", M_User.getUserId()),
                new OracleParameter("@updateby", M_User.getUserId())
                );

            return organization;
        }

        public static void InsertUpdateDelete(List<M_Organization> organization, OracleConnection sqlConnection, OracleTransaction sqlTransaction)
        {
            DataTable dt = organization.ToDataTable();

            //generate temporary table
            string table_name = Crypto.RandomString(10);
            string sql = string.Format(@"
                CREATE TABLE #{0}
                (
                user_id INT,
                user_root INT,
                can_approve BIT,
                division_id INT
                )
                ", table_name);

            SqlCommand createTempTable = new SqlCommand(sql, sqlConnection, sqlTransaction);
            createTempTable.ExecuteNonQuery();

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.KeepIdentity, sqlTransaction))
            {
                bulkCopy.DestinationTableName = string.Format("#{0}", table_name);
                bulkCopy.ColumnMappings.Add("user_id", "user_id");
                bulkCopy.ColumnMappings.Add("user_root", "user_root");
                bulkCopy.ColumnMappings.Add("can_approve", "can_approve");
                bulkCopy.ColumnMappings.Add("division_id", "division_id");                
                try
                {
                    bulkCopy.WriteToServer(dt);
                }
                catch (Exception ex)
                {
                    sqlTransaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }

            string merge = string.Format(@"                
                MERGE INTO dbo.m_organization WITH(HOLDLOCK) AS T
                USING #{0} AS S
                ON (T.user_id=S.user_id AND T.user_root=S.user_root)
                WHEN MATCHED THEN 
                UPDATE SET can_approve=S.can_approve, division_id=S.division_id, user_root=S.user_root, updateby=@updateby, updatedate=GETDATE()
                WHEN NOT MATCHED BY TARGET THEN
                INSERT(division_id, can_approve, user_id, user_root, createby, createdate)
                VALUES(S.division_id, can_approve, S.user_id, S.user_root, @createby, GETDATE())
                ;
                UPDATE dbo.m_organization SET isdelete=1, deleteby=@deleteby, deletedate=GETDATE()
                WHERE user_id=@user_id AND user_root NOT IN (SELECT user_root FROM #{0})", table_name);

            int user_id = organization.Take(1).Select(x => x.user_id).FirstOrDefault().ToString().ToInteger();

            SqlCommand mergeTable = new SqlCommand(merge, sqlConnection, sqlTransaction);
            mergeTable.Parameters.AddWithValue("@updateby", M_User.getUserId());
            mergeTable.Parameters.AddWithValue("@createby", M_User.getUserId());
            mergeTable.Parameters.AddWithValue("@user_id", user_id);
            mergeTable.ExecuteNonQuery();

            SqlCommand deleteTempTable = new SqlCommand(string.Format("DROP TABLE #{0}", table_name), sqlConnection, sqlTransaction);
            deleteTempTable.ExecuteNonQuery();            
        }

        public static M_Organization Delete(M_Organization data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_organization 
                SET 
                isdelete=@isdelete, 
                deletedate=GETDATE(),
                deleteby=@deleteby
                WHERE organization_id=@organization_id;",
                connection, transaction,
                new OracleParameter("@isdelete", true),
                new OracleParameter("@organization_id", data.organization_id),
                new OracleParameter("@deleteby", M_User.getUserId())
                );

            return data;
        }

        public static M_Organization Disable(M_Organization data, OracleConnection connection, OracleTransaction transaction)
        {
            string sql = string.Format(@"UPDATE m_organization
                SET 
                isactive=@isactive, 
                updatedate=GETDATE(),
                updateby=@updateby
                WHERE organization_id=@organization_id");

            Database.querySQL(sql,
                connection, transaction,
                new OracleParameter("@isactive", false),
                new OracleParameter("@organization_id", data.organization_id),
                new OracleParameter("@updateby", M_User.getUserId())
                );

            return data;
        }

        public static M_Organization Enable(M_Organization data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_organization 
                SET 
                isactive=@isactive, 
                updatedate=GETDATE(),
                updateby=@updateby
                WHERE organization_id=@organization_id;",
                connection, transaction,
                new OracleParameter("@isactive", true),
                new OracleParameter("@organization_id", data.organization_id),
                new OracleParameter("@updateby", M_User.getUserId())
                );

            return data;
        }

        public static bool IsExist(M_Organization data)
        {
            DataTable dt = Database.getDataTable(@"
                SELECT * FROM m_organization (NOLOCK) 
                WHERE division_id=@division_id
                AND user_id=@user_id
                AND user_root=@user_root",
                new OracleParameter("@division_id", data.division_id),
                new OracleParameter("@user_id", data.user_id),
                new OracleParameter("@user_root", data.user_root));

            if (dt.Rows.Count > 0) return true;

            return false;
        }

        public static DataTable SelectAll()
        {
            string sql = string.Format(@"SELECT O.organization_id, U.user_id, U.username, U.fullname, COALESCE(O.user_root, 0) user_root, Di.department_id, O.division_id, 
                COALESCE(De.department_name,'-')department_name, COALESCE(Di.division_name,'-')division_name, O.can_approve 
                FROM dbo.m_user (NOLOCK) U
                LEFT JOIN dbo.m_organization (NOLOCK) O ON U.user_id=O.user_id                
                LEFT JOIN dbo.m_division (NOLOCK) Di ON O.division_id=Di.division_id
                LEFT JOIN dbo.m_department (NOLOCK) De ON Di.department_id=De.department_id
                WHERE U.company_id=@company_id AND U.isdelete=0 
                ORDER BY U.user_id
                ", M_User.getDatabaseName());

            DataTable dt = Database.getDataTable(sql, new OracleParameter("@company_id", M_User.getCompanyId()));
            return dt;
        }
    }
}