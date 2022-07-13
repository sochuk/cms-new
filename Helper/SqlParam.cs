using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMS.Helper
{
    public static class SqlParam
    {
        public static JObject toJObject(this OracleParameter[] param)
        {
            JObject json = new JObject();
            foreach (var a in param)
            {
                //Remove @ start of parameter
                json.Add(a.ParameterName.Remove(0, 1), JToken.FromObject(a.Value));
            }

            return json;
        }
    }
}