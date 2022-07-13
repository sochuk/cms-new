using CMS.Helper;
using CMS.Management;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CMS.Management.Model
{
    public class RoleCustomManager : RoleProvider
    {        
        public override string[] GetRolesForUser(string username)
        {
            //username = HttpContext.Current.User.Identity.Get_GroupName();
            //DataTable dt = Database.getDataTable(@"
            //        SELECT R.role_name FROM m_role  R
            //        LEFT JOIN m_group  G ON R.role_id=G.role_id
            //        WHERE G.group_name=:group_name",
            //        new System.Data.SqlClient.OracleParameter(":group_name", username));
            //return dt.toStringSingleArray("role_name");

            string[] role = new string[] { HttpContext.Current.User.Identity.Get_RoleName().ToString() };
            return role;
        }

        public override string[] GetAllRoles()
        {
            DataTable dt =  Database.getDataTable("SELECT * FROM m_role ");
            List<M_Role> list_role = new List<M_Role>();
            foreach (DataRow row in dt.Rows)
            {
                list_role.Add(new M_Role
                {
                    role_id = Convert.ToInt32(row["role_id"].ToString()),
                    role_name = row["role_name"].ToString(),
                    role_desc = row["role_desc"].ToString()
                }
                );
            }
            return dt.ToStringSingleArray("role_name");
        }

        public override void AddUsersToRoles(string[] usernames, string[] rolenames)
        {
        }

        public override string[] GetUsersInRole(string rolename)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string rolename, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string rolename)
        {

        }

        public override bool RoleExists(string rolename)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string rolename)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] rolenames)
        {

        }
    }
}