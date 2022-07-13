using Newtonsoft.Json.Linq;
using KMS.Helper;
using KMS.Management.Model;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace KMS.Management.Structure.Model
{
    public class M_Department
    {
        public int department_id { get; set; }
        public string department_name { get; set; }
        public string department_desc { get; set; }
        public bool isactive { get; set; }
        public bool isdelete { get; set; }

        public static M_Department Insert(M_Department department, OracleConnection connection, OracleTransaction transaction)
        {
            DataTable dt = Database.queryScalar(@"
                INSERT INTO m_department(department_name, department_desc, company_id, createby, createdate)
                VALUES(@department_name, @department_desc, @createby, @company_id, GETDATE());
                SELECT @@IDENTITY AS 'id';", 
                connection, transaction,
                out OracleParameter[] param_out,
                new OracleParameter("@department_name", department.department_name),
                new OracleParameter("@department_desc", department.department_desc),
                new OracleParameter("@company_id", M_User.getCompanyId()),
                new OracleParameter("@createby", M_User.getUserId())
                );

            department.department_id = dt.Rows[0]["id"].ToString().ToInteger();
            return department;
        }

        public static M_Department Update(M_Department department, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_department 
                SET department_name=@department_name, 
                department_desc=@department_desc,
                company_id=@company_id,
                updateby=@updateby,
                updatedate=GETDATE()
                WHERE department_id=@department_id;",
                connection, transaction,
                out OracleParameter[] param_out,
                new OracleParameter("@department_id", department.department_id),
                new OracleParameter("@department_name", department.department_name),
                new OracleParameter("@department_desc", department.department_desc),
                new OracleParameter("@company_id", M_User.getCompanyId()),
                new OracleParameter("@updateby", M_User.getUserId())
                );

            return department;
        }

        public static M_Department Delete(M_Department data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_department 
                SET 
                isdelete=@isdelete, 
                deletedate=GETDATE(),
                deleteby=@deleteby
                WHERE department_id=@department_id;",
                connection, transaction,
                out OracleParameter[] param_out,
                new OracleParameter("@isdelete", true),
                new OracleParameter("@department_id", data.department_id),
                new OracleParameter("@deleteby", M_User.getUserId())
                );

            return data;
        }

        public static M_Department Disable(M_Department data, OracleConnection connection, OracleTransaction transaction)
        {
            string sql = string.Format(@"UPDATE m_department
                SET 
                isactive=@isactive, 
                updatedate=GETDATE(),
                updateby=@updateby
                WHERE department_id=@department_id");

            Database.querySQL(sql, 
                connection, transaction,
                out OracleParameter[] param_out,
                new OracleParameter("@isactive", false),
                new OracleParameter("@department_id", data.department_id),
                new OracleParameter("@updateby", M_User.getUserId())
                );

            return data;
        }

        public static M_Department Enable(M_Department data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_department 
                SET 
                isactive=@isactive, 
                updatedate=GETDATE(),
                updateby=@updateby
                WHERE department_id=@department_id;",
                connection, transaction,
                out OracleParameter[] param_out,
                new OracleParameter("@isactive", true),
                new OracleParameter("@department_id", data.department_id),
                new OracleParameter("@updateby", M_User.getUserId())
                );
            
            return data;
        }

        public static bool IsExist(M_Department data)
        {
            DataTable dt = Database.getDataTable("SELECT * FROM m_department (NOLOCK) where department_name=@department_name",
                new OracleParameter("@department_name", data.department_name));

            if (dt.Rows.Count > 0) return true;

            return false;
        }

        public static DataTable SelectAll()
        {
            
            string sql = string.Format(@"SELECT * from m_department WHERE isdelete=0 AND company_id=@company_id", M_User.getDatabaseName());
            DataTable dt = Database.getDataTable(sql, new OracleParameter("@company_id", M_User.getCompanyId()));
            return dt;
        }
    }
}