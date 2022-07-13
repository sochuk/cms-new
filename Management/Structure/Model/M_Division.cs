using KMS.Helper;
using KMS.Management.Model;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace KMS.Management.Structure.Model
{
    public class M_Division
    {
        public int division_id { get; set; }
        public string division_name { get; set; }
        public string division_desc { get; set; }
        public int department_id { get; set; }
        public bool isactive { get; set; }
        public bool isdelete { get; set; }

        public static M_Division Insert(M_Division division, OracleConnection connection, OracleTransaction transaction)
        {
            DataTable dt = Database.queryScalar(@"
                INSERT INTO m_division(division_name, division_desc, department_id, company_id, createby, createdate)
                VALUES(@division_name, @division_desc, @department_id, @company_id, @createby, GETDATE());
                SELECT @@IDENTITY AS 'id';", 
                connection, transaction,
                out OracleParameter[] param_out,
                new OracleParameter("@division_name", division.division_name),
                new OracleParameter("@division_desc", division.division_desc),
                new OracleParameter("@department_id", division.department_id),
                new OracleParameter("@company_id", M_User.getCompanyId()),
                new OracleParameter("@createby", M_User.getUserId())
                );

            division.division_id = dt.Rows[0]["id"].ToString().ToInteger();

            return division;
        }

        public static M_Division Update(M_Division division, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_division 
                SET division_name=@division_name, 
                division_desc=@division_desc,
                department_id=@department_id,
                company_id=@company_id,
                updateby=@updateby,
                updatedate=GETDATE()
                WHERE division_id=@division_id;",
                connection, transaction,
                out OracleParameter[] param_out,
                new OracleParameter("@division_id", division.division_id),
                new OracleParameter("@division_name", division.division_name),
                new OracleParameter("@division_desc", division.division_desc),
                new OracleParameter("@department_id", division.department_id),
                new OracleParameter("@company_id", M_User.getCompanyId()),
                new OracleParameter("@updateby", M_User.getUserId())
                );

            return division;
        }

        public static M_Division Delete(M_Division data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_division 
                SET 
                isdelete=@isdelete, 
                deletedate=GETDATE(),
                deleteby=@deleteby
                WHERE division_id=@division_id;",
                connection, transaction,
                out OracleParameter[] param_out,
                new OracleParameter("@isdelete", true),
                new OracleParameter("@division_id", data.division_id),
                new OracleParameter("@deleteby", M_User.getUserId())
                );

            return data;
        }

        public static M_Division Disable(M_Division data, OracleConnection connection, OracleTransaction transaction)
        {
            string sql = string.Format(@"UPDATE m_division
                SET 
                isactive=@isactive, 
                updatedate=GETDATE(),
                updateby=@updateby
                WHERE division_id=@division_id");

            Database.querySQL(sql, 
                connection, transaction,
                out OracleParameter[] param_out,
                new OracleParameter("@isactive", false),
                new OracleParameter("@division_id", data.division_id),
                new OracleParameter("@updateby", M_User.getUserId())
                );

            return data;
        }

        public static M_Division Enable(M_Division data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_division 
                SET 
                isactive=@isactive, 
                updatedate=GETDATE(),
                updateby=@updateby
                WHERE division_id=@division_id;",
                connection, transaction,
                out OracleParameter[] param_out,
                new OracleParameter("@isactive", true),
                new OracleParameter("@division_id", data.division_id),
                new OracleParameter("@updateby", M_User.getUserId())
                );

            return data;
        }

        public static bool IsExist(M_Division data)
        {
            DataTable dt = Database.getDataTable("SELECT * FROM m_division (NOLOCK) where division_name=@division_name",
                new OracleParameter("@division_name", data.division_name));

            if (dt.Rows.Count > 0) return true;

            return false;
        }

        public static DataTable SelectAll()
        {
            string sql = string.Format(@"SELECT D.*,DE.department_name from m_division (NOLOCK) D
                    LEFT JOIN m_department (NOLOCK) DE ON D.department_id=DE.department_id
                    WHERE D.isdelete=0 AND D.company_id=@company_id", 
                    M_User.getDatabaseName());
            DataTable dt = Database.getDataTable(sql, new OracleParameter("@company_id", M_User.getCompanyId()));
            return dt;
        }
    }
}