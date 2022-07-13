using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CMS.Helper
{
    public class DateHelper
    {
        public static DataTable getDate(int year, int month)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("day", typeof(Int32));
            dt.Columns.Add("date", typeof(DateTime));

            int days = DateTime.DaysInMonth(year, month);
            for(int x = 1; x <= days; x++)
            {
                DataRow dr = dt.NewRow();
                dr["day"] = x;
                dr["date"] = Convert.ToDateTime(DateTime.ParseExact(year + "-" + month + "-" + x, "yyyy-M-d", null));
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}