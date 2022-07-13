using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;
using CMS.Helper;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;

namespace CMS.Helper
{
    public class Database
    {
        public static OracleConnection cnn;

        public static bool openConnection()
        {
            cnn = new OracleConnection();
            try
            {
                cnn.ConnectionString = Database.getConnectionString("Default");
                cnn.Open();
                return true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
        }

        public static bool closeConnection()
        {
            cnn = new OracleConnection();
            try
            {
                cnn.Close();
                return true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
        }

        public static string getConnectionString(string ConnectionString)
        {
            string cnn = System.Configuration.ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString;
            return cnn;
        }

        public static DataTable getDataTable(string CommandText, params OracleParameter[] sqlParameter)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var sqlConnection = new OracleConnection(Database.getConnectionString("Default")))
                {
                    sqlConnection.Open();
                    sqlConnection.CreateCommand();
                    using (var sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        using (var sqlCommand = new OracleCommand())
                        {
                            sqlCommand.BindByName = true;
                            sqlCommand.CommandText = CommandText;
                            sqlCommand.Connection = sqlConnection;
                            sqlCommand.Transaction = sqlTransaction;
                            if (sqlParameter.Count() > 0) sqlCommand.Parameters.AddRange(sqlParameter);

                            using (OracleDataReader reader = sqlCommand.ExecuteReader())
                            {
                                dt.Load(reader);
                                try
                                {
                                    sqlTransaction.Commit();
                                }
                                catch
                                {
                                    sqlTransaction.Rollback();
                                }
                            }
                        }
                    }
                    //sqlConnection.Close();
                }
            }catch(Exception ex)
            {

            }
            return dt.ToColumnLowerCase();
        }

        public static async Task<DataTable> getDataTableAsync(string CommandText, params OracleParameter[] sqlParameter)
        {
            DataTable dt = new DataTable();
            using (var sqlConnection = new OracleConnection(Database.getConnectionString("Default")))
            {
                await sqlConnection.OpenAsync();
                sqlConnection.CreateCommand();
                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    using (var sqlCommand = new OracleCommand())
                    {
                        sqlCommand.BindByName = true;
                        sqlCommand.CommandText = CommandText;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.Transaction = sqlTransaction;
                        if (sqlParameter.Count() > 0) sqlCommand.Parameters.AddRange(sqlParameter);

                        using (var reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            dt.Load(reader);
                        }
                    }
                }
                //sqlConnection.Close();
            }
            return dt.ToColumnLowerCase();
        }

        public static DataTable getDataTable(string CommandText, OracleConnection sqlConnection, OracleTransaction sqlTransaction, params OracleParameter[] sqlParameter)
        {
            DataTable dt = new DataTable();

            using(var sqlCommand = new OracleCommand())
            {
                sqlCommand.BindByName = true;
                sqlCommand.CommandText = CommandText;
                sqlCommand.Connection = sqlConnection;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.AddRange(sqlParameter);
                using (OracleDataReader reader = sqlCommand.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
            
            return dt.ToColumnLowerCase();
        }

        public static void querySQL(string CommandText, params OracleParameter[] sqlParameter)
        {
            DataTable dt = new DataTable();
            using (var sqlConnection = new OracleConnection(Database.getConnectionString("Default")))
            {
                sqlConnection.Open();
                sqlConnection.CreateCommand();
                using (var transaction = sqlConnection.BeginTransaction())
                {
                    using(var sqlCommand = new OracleCommand())
                    {
                        sqlCommand.BindByName = true;
                        sqlCommand.CommandText = CommandText;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.Transaction = transaction;
                        sqlCommand.Parameters.AddRange(sqlParameter);

                        try
                        {
                            sqlCommand.ExecuteNonQuery();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                            Console.WriteLine("  Message: {0}", ex.Message);
                            try
                            {
                                transaction.Rollback();
                            }
                            catch (Exception ex2)
                            {
                                Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                                Console.WriteLine("  Message: {0}", ex2.Message);
                            }

                            throw new Exception(ex.Message);
                        }
                    }                    
                }
                //sqlConnection.Close();
            }                        
        }

        public static void querySQL(string CommandText, OracleConnection sqlConnection, OracleTransaction sqlTransaction, params OracleParameter[] sqlParameter)
        {
            sqlConnection.CreateCommand();

            using (var sqlCommand = new OracleCommand())
            {
                sqlCommand.BindByName = true;
                sqlCommand.CommandText = CommandText;
                sqlCommand.Connection = sqlConnection;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.AddRange(sqlParameter);
                sqlCommand.ExecuteNonQuery();
            }
            
        }

        public static void querySQL(string CommandText, OracleConnection sqlConnection, OracleTransaction sqlTransaction, out OracleParameter[] sqlParameter_out, params OracleParameter[] sqlParameter)
        {
            sqlConnection.CreateCommand();

            using(var sqlCommand = new OracleCommand())
            {
                sqlCommand.BindByName = true;
                sqlCommand.CommandText = CommandText;
                sqlCommand.Connection = sqlConnection;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.AddRange(sqlParameter);
                sqlCommand.ExecuteNonQuery();
            }            

            sqlParameter_out = sqlParameter;
        }

        public static DataTable queryScalar(string CommandText, OracleConnection sqlConnection, OracleTransaction sqlTransaction, params OracleParameter[] sqlParameter)
        {
            DataTable dt = new DataTable();

            sqlConnection.CreateCommand();

            using (var sqlCommand = new OracleCommand())
            {
                sqlCommand.BindByName = true;
                sqlCommand.CommandText = CommandText;
                sqlCommand.Connection = sqlConnection;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.AddRange(sqlParameter);
                using (OracleDataReader reader = sqlCommand.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
            
            return dt.ToColumnLowerCase();
        }

        public static DataTable queryScalar(string CommandText, OracleConnection sqlConnection, OracleTransaction sqlTransaction, out OracleParameter[] sqlParameter_out, params OracleParameter[] sqlParameter)
        {
            DataTable dt = new DataTable();

            sqlConnection.CreateCommand();

            using (var sqlCommand = new OracleCommand())
            {
                sqlCommand.BindByName = true;
                sqlCommand.CommandText = CommandText;
                sqlCommand.Connection = sqlConnection;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.AddRange(sqlParameter);
                using (OracleDataReader reader = sqlCommand.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }            

            sqlParameter_out = sqlParameter;
            return dt.ToColumnLowerCase();
        }

        public static DataTable queryScalar(string CommandText, params OracleParameter[] sqlParameter)
        {
            DataTable dt = new DataTable();
            using (var sqlConnection = new OracleConnection(Database.getConnectionString("Default")))
            {
                sqlConnection.Open();
                sqlConnection.CreateCommand();
                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    using(var sqlCommand = new OracleCommand())
                    {
                        sqlCommand.BindByName = true;
                        sqlCommand.CommandText = CommandText;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.Transaction = sqlTransaction;
                        sqlCommand.Parameters.AddRange(sqlParameter);
                        using (OracleDataReader reader = sqlCommand.ExecuteReader())
                        {
                            dt.Load(reader);
                            sqlTransaction.Commit();
                        }
                    }
                    
                }
                //sqlConnection.Close();
            }
            
            return dt.ToColumnLowerCase();
        }

        public static List<T> querySQL<T>(string CommandText, OracleConnection sqlConnection, OracleTransaction sqlTransaction, params OracleParameter[] sqlParameter)
        {
            List<T> result = null;
            sqlConnection.CreateCommand();

            using (var sqlCommand = new OracleCommand())
            {
                sqlCommand.BindByName = true;
                sqlCommand.CommandText = CommandText;
                sqlCommand.Connection = sqlConnection;
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.AddRange(sqlParameter);
                using (XmlReader reader = sqlCommand.ExecuteXmlReader())
                {
                    while (reader.Read())
                    {
                        string s = reader.Value.ToString();
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        result = JsonConvert.DeserializeObject<List<T>>(s);
                    }
                }
            }                
               
            return result;
        }

        public static long executeScalar(string field_id, string CommandText, params OracleParameter[] oracleParameter)
        {
            // Example :
            // "INSERT INTO table(name) VALUES ('Name') RETURNING id INTO :id"

            long id = 0;

            using (var oracleConnection = new OracleConnection(Database.getConnectionString("Default")))
            {
                oracleConnection.Open();
                oracleConnection.CreateCommand();
                using (var oracleTransaction = oracleConnection.BeginTransaction())
                {
                    using (var oracleCommand = new OracleCommand())
                    {
                        oracleCommand.CommandText = CommandText;
                        oracleCommand.CommandText += string.Format(" RETURNING {0} INTO :id", field_id);
                        oracleCommand.Connection = oracleConnection;
                        oracleCommand.Transaction = oracleTransaction;
                        oracleCommand.Parameters.AddRange(oracleParameter);
                        oracleCommand.Parameters.Add(new OracleParameter
                        {
                            ParameterName = ":id",
                            OracleDbType = OracleDbType.Int64,
                            Direction = ParameterDirection.Output
                        });

                        oracleCommand.ExecuteNonQuery();
                        var outParameter = oracleCommand.Parameters[":id"].Value.ToString();
                        id = Convert.ToInt64(outParameter);

                    }

                }
                oracleConnection.Close();
            }

            return id;
        }

        public static long executeScalar(string field_id, string CommandText, OracleConnection oracleConnection, OracleTransaction oracleTransaction, params OracleParameter[] oracleParameter)
        {
            // Example :
            // "INSERT INTO table(name) VALUES ('Name') RETURNING id INTO :id"

            long id = 0;

            using (var oracleCommand = new OracleCommand())
            {
                oracleCommand.CommandText = CommandText;
                oracleCommand.CommandText += string.Format(" RETURNING {0} INTO :id", field_id);
                oracleCommand.Connection = oracleConnection;
                oracleCommand.Transaction = oracleTransaction;
                oracleCommand.Parameters.AddRange(oracleParameter);
                oracleCommand.Parameters.Add(new OracleParameter
                {
                    ParameterName = ":id",
                    OracleDbType = OracleDbType.Int64,
                    Direction = ParameterDirection.Output
                });

                oracleCommand.ExecuteNonQuery();
                var outParameter = oracleCommand.Parameters[":id"].Value.ToString();
                id = Convert.ToInt64(outParameter);

            }

            return id;
        }

    }
}