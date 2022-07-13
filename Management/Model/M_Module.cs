using CMS.Helper;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;


namespace CMS.Management.Model
{
    public class M_Module
    {
        public int module_id { get; set; }
        public string type_code { get; set; }
        public string module_name { get; set; }
        public string module_desc { get; set; }
        public int module_root { get; set; }
        public string module_icon { get; set; }
        public string module_url { get; set; }
        public string module_title { get; set; }
        public bool isactive { get; set; }
        public bool isdelete { get; set; }
        public bool isvisible { get; set; }
        public int order_no { get; set; }

        public string module_name_group { get; set; }
        public int createby { get; set; }
        public int updateby { get; set; }
        public int deleteby { get; set; }
        public string createdate { get; set; }
        public string updatedate { get; set; }
        public string deletedate { get; set; }

        public static M_Module Insert(M_Module module, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                INSERT INTO m_module 
                (type_code,module_name, module_desc, module_root, module_icon, module_url, module_title, createby, createdate,isvisible, order_no)
                VALUES
                (:type_code,:module_name, :module_desc, :module_root, :module_icon, :module_url, :module_title, :createby, SYSDATE,:isvisible, :order_no)", connection, transaction,
                new OracleParameter(":type_code", module.type_code),
                new OracleParameter(":module_name", module.module_name),
                new OracleParameter(":module_desc", module.module_desc),
                new OracleParameter(":module_root", module.module_root),
                new OracleParameter(":module_icon", module.module_icon),
                new OracleParameter(":module_url", module.module_url),
                new OracleParameter(":module_title", module.module_title),
                new OracleParameter(":isvisible", module.isvisible.ToInteger()),
                new OracleParameter(":order_no", module.order_no),
                new OracleParameter(":createby", M_User.getUserId())
                );

            return module;
        }

        public static M_Module Update(M_Module module, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_module 
                SET type_code=:type_code,
                module_name=:module_name, 
                module_desc=:module_desc,
                module_root=:module_root,
                module_icon=:module_icon,
                module_url=:module_url,
                module_title=:module_title,
                isvisible=:isvisible,
                order_no=:order_no,
                updateby=:updateby,
                updatedate=SYSDATE 
                WHERE module_id=:module_id",
                connection, transaction,
                new OracleParameter(":module_id", module.module_id),
                new OracleParameter(":type_code", module.type_code),
                new OracleParameter(":module_name", module.module_name),
                new OracleParameter(":module_desc", module.module_desc),
                new OracleParameter(":module_root", module.module_root),
                new OracleParameter(":module_icon", module.module_icon),
                new OracleParameter(":module_url", module.module_url),
                new OracleParameter(":module_title", module.module_title),
                new OracleParameter(":isvisible", module.isvisible.ToInteger()),
                new OracleParameter(":order_no", module.order_no),
                new OracleParameter(":updateby", M_User.getUserId())
                );

            return module;
        }

        public static M_Module Delete(M_Module data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_module 
                SET 
                isdelete=:isdelete, 
                deletedate=SYSDATE,
                deleteby=:deleteby
                WHERE module_id=:module_id",
                connection, transaction,
                new OracleParameter(":isdelete", "1"),
                new OracleParameter(":module_id", data.module_id.ToString()),
                new OracleParameter(":deleteby", M_User.getUserId().ToString())
                );
            return data;
        }

        public static M_Module Disable(M_Module data, OracleConnection connection, OracleTransaction transaction)
        {
            string sql = string.Format(@"UPDATE m_module
                SET 
                isactive=:isactive, 
                updatedate=SYSDATE,
                updateby=:updateby
                WHERE module_id=:module_id");

            Database.querySQL(sql, connection, transaction,
                new OracleParameter(":isactive", "0"),
                new OracleParameter(":module_id", data.module_id.ToString()),
                new OracleParameter(":updateby", M_User.getUserId().ToString())
                );
            return data;
        }

        public static M_Module Enable(M_Module data, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE m_module 
                SET 
                isactive=:isactive, 
                updatedate=SYSDATE,
                updateby=:updateby
                WHERE module_id=:module_id",
                connection, transaction,
                new OracleParameter(":isactive", "1"),
                new OracleParameter(":module_id", data.module_id.ToString()),
                new OracleParameter(":updateby", M_User.getUserId().ToString())
                );
            return data;
        }

        public static bool IsExist(M_Module data)
        {
            DataTable dt = Database.getDataTable(@"SELECT * FROM m_module  
                    WHERE type_code=:type_code AND module_root=:module_root",
                new OracleParameter(":type_code", data.type_code),
                new OracleParameter(":module_name", data.module_name),
                new OracleParameter(":module_root", data.module_root));

            if (dt.Rows.Count > 0) return true;

            return false;
        }

        public static bool IsExist(string column, string value)
        {
            DataTable dt = Database.getDataTable("SELECT * FROM m_module  " +
                "WHERE " + column + "=:"+ value,
                new OracleParameter(":"+ value, value));

            if (dt.Rows.Count > 0) return true;

            return false;
        }

        public static DataTable SelectAll()
        {
            
            string sql = string.Format(@"
            SELECT a.module_id, a.type_code, a.module_name, a.module_desc, a.module_title, a.module_root, a.module_icon, a.module_url, a.module_type,
            DECODE(a.isactive, 0, 'False', 'True') isactive, 
            DECODE(a.isdelete, 0, 'False', 'True') isdelete,
            DECODE(a.isvisible, 0, 'False', 'True') isvisible, 
            a.order_no, a.createdate, a.updatedate, a.deletedate, a.createby, a.updateby, a.deleteby, 
            CONCAT(
            B.module_name, 
            (CASE WHEN B.module_desc = NULL THEN '' ELSE CONCAT(' (' || B.module_desc, ')') END)) AS module_name_group
            FROM m_module  A
            LEFT JOIN m_module  B ON A.module_root=B.module_id
            WHERE A.isdelete=0 ORDER BY A.order_no, A.module_name", M_User.getDatabaseName());

            DataTable dt = Database.getDataTable(sql);

            List<M_Module> module = new List<M_Module>();
            foreach(DataRow row in dt.Rows)
            {
                module.Add(
                    new M_Module()
                    {
                        module_id = row["module_id"].ToInteger(),
                        type_code = row["type_code"].ToString(),
                        module_name = row["module_name"].ToString(),
                        module_desc = row["module_desc"].ToString(),
                        module_root = row["module_root"].ToInteger(),
                        module_icon = row["module_icon"].ToString(),
                        module_url = row["module_url"].ToString(),
                        module_title = row["module_title"].ToString(),
                        isactive = row["isactive"].ToBoolean(),
                        isdelete = row["isdelete"].ToBoolean(),
                        isvisible = row["isvisible"].ToBoolean(),
                        order_no = row["order_no"].ToInteger(),
                        module_name_group = row["module_name_group"].ToString(),
                        createby= row["createby"].ToInteger(),
                        updateby = row["updateby"].ToInteger(),
                        deleteby = row["deleteby"].ToInteger(),
                        createdate = row["createdate"].ToString(),
                        updatedate = row["updatedate"].ToString(),
                        deletedate = row["deletedate"].ToString(),
                    }
                );
            }

            DataTable mdt = module.ToDataTable();

            return mdt;
        }
    }
}