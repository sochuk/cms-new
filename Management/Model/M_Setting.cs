using CMS.Helper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMS.Management.Model
{
    public class M_Setting
    {
        public int user_id { get; set; }
        public int grid_pagesize { get; set; }
        public Theme grid_theme { get; set; }
        public bool grid_zebracolor { get; set; }
        public bool grid_wrap_column { get; set; }
        public bool grid_wrap_cell { get; set; }
        public bool grid_showfilterrow { get; set; }
        public bool grid_showfilterbar { get; set; }
        public bool grid_selectbyrow { get; set; }
        public bool grid_focuserow { get; set; }
        public bool grid_ellipsis { get; set; }
        public bool grid_showfooter { get; set; }
        public bool grid_responsive { get; set; }

        public static M_Setting Insert(M_Setting data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                INSERT INTO m_setting (user_id,grid_pagesize,grid_theme,grid_zebracolor,grid_wrap_column,grid_wrap_cell,grid_showfilterrow,grid_showfilterbar,grid_selectbyrow,grid_focuserow,grid_ellipsis,grid_showfooter,grid_responsive)
                VALUES (:user_id,:grid_pagesize,:grid_theme,:grid_zebracolor,:grid_wrap_column,:grid_wrap_cell,:grid_showfilterrow,:grid_showfilterbar,:grid_selectbyrow,:grid_focuserow,:grid_ellipsis,:grid_showfooter,:grid_responsive)", connection, transaction,
                new OracleParameter(":user_id", data.user_id.ToInteger()),
                new OracleParameter(":grid_pagesize", data.grid_pagesize.ToInteger()),
                new OracleParameter(":grid_theme", (int)data.grid_theme),
                new OracleParameter(":grid_zebracolor", data.grid_zebracolor.ToInteger()),
                new OracleParameter(":grid_wrap_column", data.grid_wrap_column.ToInteger()),
                new OracleParameter(":grid_wrap_cell", data.grid_wrap_cell.ToInteger()),
                new OracleParameter(":grid_showfilterrow", data.grid_showfilterrow.ToInteger()),
                new OracleParameter(":grid_showfilterbar", data.grid_showfilterbar.ToInteger()),
                new OracleParameter(":grid_selectbyrow", data.grid_selectbyrow.ToInteger()),
                new OracleParameter(":grid_focuserow", data.grid_focuserow.ToInteger()),
                new OracleParameter(":grid_ellipsis", data.grid_ellipsis.ToInteger()),
                new OracleParameter(":grid_showfooter", data.grid_showfooter.ToInteger()),
                new OracleParameter(":grid_responsive", data.grid_responsive.ToInteger())
                );

            return data;
        }

        public static M_Setting Update(M_Setting data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_setting 
                SET 
                grid_pagesize=:grid_pagesize, 
                grid_theme=:grid_theme,
                grid_zebracolor=:grid_zebracolor,
                grid_wrap_column=:grid_wrap_column,
                grid_wrap_cell=:grid_wrap_cell,
                grid_showfilterrow=:grid_showfilterrow,
                grid_showfilterbar=:grid_showfilterbar,
                grid_selectbyrow=:grid_selectbyrow,
                grid_focuserow=:grid_focuserow,
                grid_ellipsis=:grid_ellipsis,
                grid_showfooter=:grid_showfooter,
                grid_responsive=:grid_responsive
                WHERE user_id=:user_id",
                connection, transaction,
                new OracleParameter(":user_id", data.user_id.ToInteger()),
                new OracleParameter(":grid_pagesize", data.grid_pagesize.ToInteger()),
                new OracleParameter(":grid_theme", (int)data.grid_theme),
                new OracleParameter(":grid_zebracolor", data.grid_zebracolor.ToInteger()),
                new OracleParameter(":grid_wrap_column", data.grid_wrap_column.ToInteger()),
                new OracleParameter(":grid_wrap_cell", data.grid_wrap_cell.ToInteger()),
                new OracleParameter(":grid_showfilterrow", data.grid_showfilterrow.ToInteger()),
                new OracleParameter(":grid_showfilterbar", data.grid_showfilterbar.ToInteger()),
                new OracleParameter(":grid_selectbyrow", data.grid_selectbyrow.ToInteger()),
                new OracleParameter(":grid_focuserow", data.grid_focuserow.ToInteger()),
                new OracleParameter(":grid_ellipsis", data.grid_ellipsis.ToInteger()),
                new OracleParameter(":grid_showfooter", data.grid_showfooter.ToInteger()),
                new OracleParameter(":grid_responsive", data.grid_responsive.ToInteger())
                );

            return data;
        }

        public static bool IsExist(M_Setting data)
        {
            DataTable dt = Database.getDataTable("SELECT * FROM m_setting  where user_id=:user_id",
                new OracleParameter(":user_id", data.user_id));

            if (dt.Rows.Count > 0) return true;

            return false;
        }

        public static DataTable SelectAll()
        {
            
            string sql = String.Format(@"SELECT * FROM m_setting  WHERE user_id=:user_id");
            DataTable dt = Database.getDataTable(sql, new OracleParameter("user_id", HttpContext.Current.User.Identity.Get_Id()));
            return dt;
        }

        public enum Theme
        {
            [Description("Aqua")]
            Aqua,
            [Description("Black Glass")]
            BlackGlass,
            [Description("Default")]
            Default,
            [Description("Devex")]
            DevEx,
            [Description("Glass")]
            Glass,
            [Description("Metropolis")]
            Metropolis,
            [Description("Metropolis Blue")]
            MetropolisBlue,
            [Description("Office 2003 Blue")]
            Office2003Blue,
            [Description("Office 2003 Olive")]
            Office2003Olive,
            [Description("Office 2003 Silver")]
            Office2003Silver,
            [Description("Office 2010 Blue")]
            Office2010Blue,
            [Description("Office 2010 Black")]
            Office2010Black,
            [Description("Office 2010 Silver")]
            Office2010Silver,
            [Description("Plastic Blue")]
            PlasticBlue,
            [Description("Soft Orange")]
            SoftOrange,
            [Description("Youthful")]
            Youthful,
            [Description("Material")]
            Material,
            [Description("MaterialCompact")]
            MaterialCompact
        }
    }
}