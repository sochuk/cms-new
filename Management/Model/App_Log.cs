using CMS.Helper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMS.Management.Model
{
    public class App_Log
    {
        public int log_id { get; set; }
        public string log_title { get; set; }
        public string log_content { get; set; }

        public void Save()
        {
            using (var sqlConnection = new OracleConnection(Database.getConnectionString("Default")))
            {
                sqlConnection.Open();
                using (OracleTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    Database.querySQL(@"
                    INSERT INTO log_app 
                    (
                        log_title,
                        log_content,
                        browser,
                        createdate
                    )
                    VALUES
                    (
                        :log_title,
                        :log_content,
                        :browser,
                        SYSDATE
                    )",
                    sqlConnection, sqlTransaction,
                        new OracleParameter(":log_title", this.log_title),
                        new OracleParameter(":log_content", this.log_content),
                        new OracleParameter(":browser", HttpContext.Current.Request.Browser.Browser)
                    );
                    sqlTransaction.Commit();
                }
            }
        }
    }
}