using CMS.Management.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace CMS.Helper
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple =true, Inherited =true)]
    public class NeedAccessRight : Attribute
    {
        public NeedAccessRight()
        {
            IsAllowed();
        }

        public bool IsAllowed()
        {
            if (HttpContext.Current.User.Identity.Get_RoleName() == "Super Administrator") return true;

            string abs_path = HttpContext.Current.Request.Url.AbsolutePath;
            string path = HttpContext.Current.Request.ApplicationPath;
            abs_path = (path != "/" ? abs_path.Replace(path, null) : abs_path);

            DataTable isallowed = Database.getDataTable(@"
                        SELECT * FROM m_access A
                        LEFT JOIN m_module M ON A.module_id=M.module_id
                        WHERE M.module_url=:module_url AND A.group_id=:group_id",
                        new OracleParameter(":module_url", abs_path),
                        new OracleParameter(":group_id", HttpContext.Current.User.Identity.Get_GroupID())
                );
            
            if (isallowed.Rows.Count == 0)
            {
                HttpContext.Current.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                HttpContext.Current.Response.TrySkipIisCustomErrors = true;
                throw new Exception("Forbidden Access").SetCode(-2146233078);
            }
            else
            {
                if (!Convert.ToBoolean(isallowed.Rows[0]["isactive"].ToBoolean()))
                {
                    HttpContext.Current.Response.TrySkipIisCustomErrors = true;
                    throw new Exception("Under Maintenance").SetCode(0);
                }
            }
            return true;
        }
    }
}