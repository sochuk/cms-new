using CMS.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CMS.Management.Model
{
    public class M_Icon
    {
        public string icon_code { get; set; }
        public string icon_name { get; set; }

        public static DataTable SelectAll()
        {
            string sql = string.Format(@"SELECT icon_name, icon_code, CONCAT('{0}' || icon_code, '{1}') AS icon FROM m_icon ", "<i class=\"", "\"></i>");
            DataTable dt = Database.getDataTable(sql);
            return dt;
        }
    }
}