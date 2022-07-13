using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CMS.Helper
{
    public static class DataTables
    {
        public static DataTable ResolvePhotoUrl(this DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["photo"].ToString() == null || row["photo"].ToString() == string.Empty)
                    {
                        row["photo"] = Application.GetPath() + "/Content/images/unknown.jpg";
                    }
                    else
                    {
                        if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(row["photo"].ToString())))
                        {
                            row["photo"] = Application.GetPath() + "/Content/images/unknown.jpg";
                        }
                    }
                }
            }

            return dt;
        }

        public static DataTable ToColumnLowerCase(this DataTable dt)
        {
            foreach (DataColumn column in dt.Columns)
                column.ColumnName = column.ColumnName.ToLower();
            dt.AcceptChanges();
            return dt;
        }
    }
}