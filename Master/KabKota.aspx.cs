using CMS.Context;
using CMS.Helper;
using CMS.Hubs;
using CMS.Management.Model;
using CMS.Master.Model;
using CMS.Notification;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Web;
using GenericParsing;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMS.Master
{
    public partial class KabKota : CPanel
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            grid.SettingsExport.FileName = Page.Title + " (" + DateTime.Now.ToString("yyyyMMddHHmmss") + ")";
        }

        protected void grid_Init(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            grid.initializeGrid(false);
        }

        protected void grid_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            grid.initializeHtmlRowPrepared(e);
        }

        protected void KabKotaData_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "ID";
            var db = new CMSContext();
            var data = db.M_KABKOTA;
            e.QueryableSource = data;
        }

        protected void grid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            grid = (ASPxGridView)sender;
            using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
            {
                cnn.Open();
                using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                {
                    M_KabKota kabKota = new M_KabKota();
                    try
                    {
                        if (null != e.NewValues["PROVINSI_ID"])
                        {
                            kabKota.PROVINSI_ID = int.Parse((string)e.NewValues["PROVINSI_ID"]);
                        }

                    }catch(Exception exx)
                    {

                    }

                    kabKota.CODE = (string)e.NewValues["NAMECODE"];
                    kabKota.NAME = (string)e.NewValues["NAME"];
                    kabKota.TYPE = (string)e.NewValues["TYPE"];
                    try
                    {
                        kabKota = M_KabKota.Insert(kabKota, cnn, sqlTransaction);
                        e.NewValues.Add(nameof(kabKota.ID), kabKota.ID);
                        Log.Insert(Log.LogType.ADD, "Add kabKota", JObject.FromObject(e.NewValues), cnn, sqlTransaction);
                        sqlTransaction.Commit();
                        grid.alertSuccess();
                    }
                    catch (Exception ex)
                    {
                        ex.Message.ToString();
                        grid.alertError(ex.Message);
                        sqlTransaction.Rollback();
                        throw new Exception(ex.Message);
                    }

                    (sender as ASPxGridView).CancelEdit();
                }
                cnn.Close();
            }
            e.Cancel = true;
        }

        protected void grid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            grid = (ASPxGridView)sender;
            using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
            {
                cnn.Open();
                using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                {
                    M_KabKota kabKota = new M_KabKota();
                    kabKota.ID = e.Keys["ID"].ToInteger();
                    try{
                        if (null != e.NewValues["PROVINSI_ID"])
                        {
                            kabKota.PROVINSI_ID = int.Parse((string)e.NewValues["PROVINSI_ID"]);
                        }

                    }catch (Exception exx)
                    {

                    }

                    kabKota.CODE = (string)e.NewValues["NAMECODE"];
                    kabKota.NAME = (string)e.NewValues["NAME"];
                    kabKota.TYPE = (string)e.NewValues["TYPE"];

                    try
                    {
                        M_KabKota.Update(kabKota, cnn, sqlTransaction);
                        e.NewValues.Add(nameof(kabKota.ID), kabKota.ID);
                        Log.Insert(Log.LogType.UPDATE, "Update kabKota", JObject.FromObject(e.NewValues), cnn, sqlTransaction);
                        sqlTransaction.Commit();
                        grid.alertSuccess();
                    }
                    catch (Exception ex)
                    {
                        ex.Message.ToString();
                        grid.alertError(ex.Message.ToString());
                        sqlTransaction.Rollback();
                        throw new Exception(ex.Message);
                    }

                    (sender as ASPxGridView).CancelEdit();
                }
                cnn.Close();
            }
            e.Cancel = true;
        }

        private void ExportCSV()
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<UserHub>();
            string username = M_User.getUsername();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            hub.Clients.User(username).started();
            hub.Clients.User(username).getProgress("Please wait while loading data to export. This process take a few minutes ...");

            DevExpress.XtraPrinting.CsvExportOptionsEx options = new DevExpress.XtraPrinting.CsvExportOptionsEx();
            //gridExporter.WriteCsvToResponse("Log Report - " + DateTime.Now.ToString("dd-MM-yyyy HH.mm.ss"), true, options);

            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment; filename=" + Page.Title + " - " + DateTime.Now.ToString("dd-MM-yyyy HH.mm.ss") + ".csv");
                gridExporter.WriteCsv(memoryStream, options);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();

                hub.Clients.User(username).finished();
                Alert alert = new Alert("Success", "Data downloaded successfully in " + (stopwatch.ElapsedMilliseconds / 1000) + " seconds", Alert.TypeMessage.Success);
                hub.Clients.User(username).success(alert.ToString());
                GC.Collect();

                Response.End();
            }
        }
        private void ExportXLSX()
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<UserHub>();
            string username = M_User.getUsername();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            hub.Clients.User(username).started();
            hub.Clients.User(username).getProgress("Please wait while loading data to export. This process take a few minutes ...");

            var op = CriteriaOperator.Parse(grid.FilterExpression);
            var result = CriteriaToWhereClauseHelper.GetOracleWhere(op);

            var linqExpression = CriteriaToQueryableExtender.AppendWhere(new CMSContext().M_PROVINSI, new CriteriaToEFExpressionConverter(), op);

            int takeData = 1000000;
            int rowCount = grid.VisibleRowCount;
            double c = Convert.ToDouble(rowCount / 1000000.00);
            var divideSheet = Math.Ceiling(c);

            List<M_KABKOTA> data = new List<M_KABKOTA>();
            var users = M_User.SelectAll().AsEnumerable();

            int i = 0;
            int dataCount = linqExpression.Count();
            foreach (M_KABKOTA item in linqExpression)
            {
                data.Add(item);
                string percent = (Math.Round((i * .001f) / (dataCount * .001f) * 100.0f, 1)).ToString();
                hub.Clients.User(username).getProgress("Please wait while generating class data " + i + " of " + dataCount + " (" + percent + "% progress). Elapsed time : " + stopwatch.Elapsed);
                i++;
            }

            if (divideSheet == 1)
            {
                i = 1;

                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;

                //Header of table  
                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;
                workSheet.Cells[1, 1].Value = "PROVINSI_ID";
                workSheet.Cells[1, 2].Value = "CODE";
                workSheet.Cells[1, 3].Value = "NAME";
                workSheet.Cells[1, 4].Value = "TYPE";

                //Body of table  
                int recordIndex = 2;
                i = 1;
                foreach (var field in data)
                {
                    try
                    {
                        string PROVINSI_ID = null != field.PROVINSI_ID ? field.PROVINSI_ID.ToString() : ""; 
                        string CODE = field.CODE;
                        string NAME = field.NAME;
                        string TYPE = field.TYPE;

                        workSheet.Cells[recordIndex, 1].Value = PROVINSI_ID;
                        workSheet.Cells[recordIndex, 2].Value = CODE;
                        workSheet.Cells[recordIndex, 3].Value = NAME;
                        workSheet.Cells[recordIndex, 4].Value = TYPE;

                        string percent = (Math.Round((i * .001f) / (data.Count() * .001f) * 100.0f, 1)).ToString();
                        hub.Clients.User(username).getProgress("Generating excel data " + i + " of " + data.Count() + "(" + percent + "% progress). Elapsed time : " + stopwatch.Elapsed);

                        PROVINSI_ID = null;
                        CODE = null;
                        NAME = null;
                        TYPE = null;
                    }
                    catch (Exception ex)
                    {
                        Log.Insert(Log.LogType.VIEW, "Error download excel", new { message = ex.Message.ToString() });
                    }

                    recordIndex++;
                    i++;
                }

                workSheet.Column(1).AutoFit();
                workSheet.Column(2).AutoFit();
                workSheet.Column(3).AutoFit();
                workSheet.Column(4).AutoFit();
                workSheet.Column(5).AutoFit();

                string excelName = Page.Title + " - " + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss");
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + excelName + ".xlsx");
                    hub.Clients.User(username).getProgress("Please wait while finishing file and downloading data ...");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                }

                Response.Flush();

                hub.Clients.User(username).finished();
                Alert alert = new Alert("Success", "Data downloaded successfully in " + (stopwatch.ElapsedMilliseconds / 1000) + " seconds", Alert.TypeMessage.Success);
                hub.Clients.User(username).success(alert.ToString());

                data.Clear();
                excel.Dispose();
                data = null;
                excel = null;

                GC.Collect();

                Response.End();
            }
            else
            {
                ExcelPackage excel = new ExcelPackage();
                IEnumerable<M_KABKOTA> itemFilter;

                for (int sheet = 1; sheet <= divideSheet; sheet++)
                {
                    if (sheet == 1)
                    {
                        itemFilter = data.Take(takeData);
                    }
                    else
                    {
                        itemFilter = data.Skip((sheet - 1) * takeData).Take(takeData);
                    }

                    var workSheet = excel.Workbook.Worksheets.Add("Sheet" + sheet);
                    workSheet.DefaultRowHeight = 12;

                    //Header of table  
                    workSheet.Row(1).Height = 20;
                    workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(1).Style.Font.Bold = true;
                    workSheet.Cells[1, 1].Value = "PROVINSI_ID";
                    workSheet.Cells[1, 2].Value = "CODE";
                    workSheet.Cells[1, 3].Value = "NAME";
                    workSheet.Cells[1, 4].Value = "TYPE";

                    //Body of table  
                    int recordIndex = 2;
                    int filterCount = itemFilter.Count();
                    i = 0;
                    foreach (var field in itemFilter)
                    {
                        try
                        {
                            string PROVINSI_ID = null != field.PROVINSI_ID ? field.PROVINSI_ID.ToString() : "";
                            string CODE = field.CODE;
                            string NAME = field.NAME;
                            string TYPE = field.TYPE;

                            workSheet.Cells[recordIndex, 1].Value = PROVINSI_ID;
                            workSheet.Cells[recordIndex, 2].Value = CODE;
                            workSheet.Cells[recordIndex, 3].Value = NAME;
                            workSheet.Cells[recordIndex, 4].Value = TYPE;

                            string percent = (Math.Round((i * .001f) / (filterCount * .001f) * 100.0f, 1)).ToString();
                            hub.Clients.User(username).getProgress("Generating sheet data " + sheet + " of " + divideSheet + ". Row " + i + " of " + filterCount + " (" + percent + "% progress). Elapsed time : " + stopwatch.Elapsed);

                            PROVINSI_ID = null;
                            CODE = null;
                            NAME = null;
                            TYPE = null;

                        }
                        catch (Exception ex)
                        {
                            Log.Insert(Log.LogType.VIEW, "Error download excel", new { message = ex.Message.ToString() });
                        }

                        recordIndex++;
                        i++;
                    }

                    workSheet.Column(1).Width = 22;
                    workSheet.Column(2).Width = 20;
                    workSheet.Column(3).Width = 20;
                    workSheet.Column(4).Width = 20;
                    workSheet.Column(5).Width = 20;

                }

                string excelName = Page.Title + " - " + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss");
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                    hub.Clients.User(username).getProgress("Please wait while finishing file and downloading data ...");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                }


                Response.Flush();

                hub.Clients.User(username).finished();
                Alert alert = new Alert("Success", "Data downloaded successfully in " + (stopwatch.ElapsedMilliseconds / 1000) + " seconds", Alert.TypeMessage.Success);
                hub.Clients.User(username).success(alert.ToString());

                data.Clear();
                excel.Dispose();
                data = null;
                excel = null;
                itemFilter = null;

                GC.Collect();

                Response.End();

            }
        }

        protected void grid_ToolbarItemClick(object source, DevExpress.Web.Data.ASPxGridViewToolbarItemClickEventArgs e)
        {
            switch (e.Item.Name.ToUpper())
            {
                case "CSVEXPORT":
                    ExportCSV();
                    break;

                case "XLSXEXPORT":
                    ExportXLSX();
                    break;

                default:
                    break;
            }

        }

        protected void CSVEXPORT_Click(object sender, EventArgs e)
        {
            ExportCSV();
        }

        protected void XLSXEXPORT_Click(object sender, EventArgs e)
        {
            ExportXLSX();
        }


        private Stopwatch stopwatch = new Stopwatch();
        protected void UploadButton_Click(object sender, EventArgs e)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<UserHub>();
            string username = M_User.getUsername();
            stopwatch.Start();

            List<M_KabKota> kabKotas = new List<M_KabKota>();
            hub.Clients.User(username).started();
            hub.Clients.User(username).getProgress("Please wait while uploading data to server. This process take a few minutes ...");

            if (ExcelUpload.HasFile)
            {
                try
                {
                    ExcelUpload.SaveAs(Server.MapPath("~/uploads/") + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
                }catch(Exception ex)
                {
                    hub.Clients.User(username).getProgress($"Data import failed. Error: {ex.Message}");
                }

            }

            if (CSVUpload.HasFile)
            {
                try
                {
                    hub.Clients.User(username).getProgress("Please wait while parsing data. This process take a few minutes ...");

                    var path = Server.MapPath("~/uploads/") + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                    CSVUpload.SaveAs(path);
                    using (GenericParser parser = new GenericParser(path))
                    {
                        parser.ColumnDelimiter = Convert.ToChar(";");
                        parser.FirstRowHasHeader = true;
                        while (parser.Read())
                        {
                            try
                            {
                                var kabKota = new M_KabKota();
                                if(null != parser["PROVINSI_ID"])
                                {
                                    kabKota.PROVINSI_ID = int.Parse(parser["PROVINSI_ID"].ToString().Trim());
                                }
                                
                                kabKota.CODE = parser["CODE"].ToString().Trim();
                                kabKota.NAME = parser["NAME"].ToString().Trim();
                                kabKota.TYPE = parser["TYPE"].ToString().Trim();
                                kabKotas.Add(kabKota);
                            }
                            catch
                            {
                                var kabKota = new M_KabKota();
                                if (null != parser[0])
                                {
                                    kabKota.PROVINSI_ID = int.Parse(parser[0].ToString().Trim());
                                }
                                kabKota.CODE = parser[1].ToString().Trim();
                                kabKota.NAME = parser[2].ToString().Trim();
                                kabKota.TYPE = parser[3].ToString().Trim();


                                kabKotas.Add(kabKota);
                            }
                        }

                        BulkInsert(kabKotas, hub, username);
                    }


                }
                catch (Exception ex)
                {
                    hub.Clients.User(username).getProgress($"Data import failed. Error: {ex.Message}");
                }
            }

            Response.Flush();
            Response.End();
        }

        public void BulkInsert(List<M_KabKota> listData, IHubContext hub, string username)
        {
            int countData = listData.Count();

            string table = "M_KABKOTA";

            using (OracleConnection connection = new OracleConnection(Database.getConnectionString("Default")))
            {
                connection.Open();
                connection.CreateCommand();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    int[] _PROVINSI_ID = new int[countData];
                    string[] _CODE = new string[countData];
                    string[] _NAME = new string[countData];
                    string[] _TYPE = new string[countData];

                    for (int j = 0; j < countData; j++)
                    {
                        _PROVINSI_ID[j] = Convert.ToInt32(listData[j].PROVINSI_ID.ToString());
                        _CODE[j] = Convert.ToString(listData[j].CODE.ToString());
                        _NAME[j] = Convert.ToString(listData[j].NAME.ToString());
                        _TYPE[j] = Convert.ToString(listData[j].TYPE.ToString());
                    }

                    hub.Clients.User(username).getProgress($"Preparing object data...");

                    OracleParameter _PROVINSI_ID_PARAM = new OracleParameter();
                    _PROVINSI_ID_PARAM.OracleDbType = OracleDbType.Int32;
                    _PROVINSI_ID_PARAM.Value = _PROVINSI_ID;

                    OracleParameter _CODE_PARAM = new OracleParameter();
                    _CODE_PARAM.OracleDbType = OracleDbType.Varchar2;
                    _CODE_PARAM.Value = _CODE;

                    OracleParameter _NAME_PARAM = new OracleParameter();
                    _NAME_PARAM.OracleDbType = OracleDbType.Varchar2;
                    _NAME_PARAM.Value = _NAME;

                    OracleParameter _TYPE_PARAM = new OracleParameter();
                    _TYPE_PARAM.OracleDbType = OracleDbType.Varchar2;
                    _TYPE_PARAM.Value = _TYPE;

                    try
                    {
                        using (OracleCommand command = new OracleCommand())
                        {
                            command.Connection = connection;
                            command.Transaction = transaction;

                            hub.Clients.User(username).getProgress($"Preparing inserting data...");

                            string insert = string.Format(@"
                            INSERT INTO {0}
                            (
                                PROVINSI_ID,
                                CODE,
                                NAME,
                                TYPE
                            )
                            VALUES
                            (
                                :1,
                                :2,
                                :3,
                                :4
                            )
                            ", table, M_User.getUserId());

                            command.CommandText = insert;
                            command.ArrayBindCount = countData;

                            command.Parameters.Clear();
                            command.Parameters.Add(_CODE_PARAM);
                            command.Parameters.Add(_NAME_PARAM);

                            hub.Clients.User(username).getProgress($"Inserting object data...");

                            command.ExecuteNonQuery();
                            transaction.Commit();

                            hub.Clients.User(username).getProgress($"Clearing object data...");
                            System.Threading.Thread.Sleep(500);
                            hub.Clients.User(username).getProgress($"Processing data finished. Elapsed time {stopwatch.Elapsed}");
                            System.Threading.Thread.Sleep(1000);
                            hub.Clients.User(username).getProgress($"Data uploaded successfully. Ellapsed time {stopwatch.Elapsed}");

                        }
                    }
                    catch (Exception ex)
                    {
                        hub.Clients.User(username).getProgress($"Processing data failed. Elapsed time {stopwatch.Elapsed} s. Error : {ex.Message}");
                        transaction.Rollback();
                        System.Threading.Thread.Sleep(1000);
                    }

                    _PROVINSI_ID_PARAM = null;
                    _CODE_PARAM = null;
                    _NAME_PARAM = null;
                    _TYPE_PARAM = null;

                    _PROVINSI_ID = null;
                    _CODE = null;
                    _NAME = null;
                    _TYPE = null;
                }
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(stream: memoryStream))
                {
                    memoryStream.Position = 0;
                    streamWriter.WriteLine("");
                    streamWriter.WriteLine("Data uploaded successfully by {0} at {1}", M_User.getFullname(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    streamWriter.WriteLine("Data count: {0} rows", listData.Count());
                    streamWriter.WriteLine("Ellapsed time: {0} seconds", stopwatch.ElapsedMilliseconds / 1000);
                    streamWriter.Flush();
                    memoryStream.WriteTo(Response.OutputStream);
                }

                Response.ContentType = "text/plain";
                Response.AddHeader("content-disposition", "attachment; filename=Import Log " + Page.Title + ".log");

                listData.Clear();
                listData = null;

                hub.Clients.User(username).finished();
                Alert alert = new Alert("Success", "Data uploaded successfully in " + (stopwatch.ElapsedMilliseconds / 1000) + " seconds", Alert.TypeMessage.Success);
                hub.Clients.User(username).success(alert.ToString());

                Response.Flush();
            }
        }

    }  

}