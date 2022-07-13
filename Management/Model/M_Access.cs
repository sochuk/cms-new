using CMS.Helper;
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;

namespace CMS.Management.Model
{
    public class M_Access
    {
        public int module_id { get; set; }
        public int group_id { get; set; }

        public static M_Access Insert(M_Access access, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                INSERT INTO m_access(module_id,group_id) VALUES(:password, :group_id)", connection, transaction,
                new OracleParameter(":password", access.module_id),
                new OracleParameter(":group_id", access.group_id)
                );

            return access;
        }

        public static M_Access Delete(M_Access access, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                DELETE FROM m_access                 
                WHERE group_id=:group_id",
                connection, transaction,
                new OracleParameter(":group_id", access.group_id)
                );

            return access;
        }
    }
}