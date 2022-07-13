using CMS.Context;
using CMS.Helper;
using CMS.Hubs;
using CMS.Management.Model;
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
    public partial class Card : CPanel
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            (grid.Columns["CREATEBY"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_User.SelectAll();
            (grid.Columns["UPDATEBY"] as GridViewDataComboBoxColumn).PropertiesComboBox.DataSource = M_User.SelectAll();
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

        protected void CardData_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "CARD_ID";
            var db = new CMSContext();
            var data = db.M_CARD;
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
                    M_Card card = new M_Card();
                    card.NIK = (string)e.NewValues["NIK"];
                    card.CARDUID = (string)e.NewValues["CARDUID"];
                    card.COMMIT_DATE = (string)e.NewValues["COMMIT_DATE"];
                    card.PROVINSI_CODE = (string)e.NewValues["PROVINSI_CODE"];
                    card.KABKOTA_CODE = (string)e.NewValues["KABKOTA_CODE"];
                    card.INNER = (string)e.NewValues["INNER"];
                    card.OUTER = (string)e.NewValues["OUTER"];

                    try
                    {
                        card = M_Card.Insert(card, cnn, sqlTransaction);
                        e.NewValues.Add(nameof(card.CARD_ID), card.CARD_ID);
                        Log.Insert(Log.LogType.ADD, "Add card", JObject.FromObject(e.NewValues), cnn, sqlTransaction);
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
                    M_Card card = new M_Card();
                    card.CARD_ID = e.Keys["CARD_ID"].ToInteger();
                    card.NIK = (string)e.NewValues["NIK"];
                    card.CARDUID = (string)e.NewValues["CARDUID"];
                    card.COMMIT_DATE = (string)e.NewValues["COMMIT_DATE"];
                    card.PROVINSI_CODE = (string)e.NewValues["PROVINSI_CODE"];
                    card.KABKOTA_CODE = (string)e.NewValues["KABKOTA_CODE"];
                    card.INNER = (string)e.NewValues["INNER"];
                    card.OUTER = (string)e.NewValues["OUTER"];

                    try
                    {
                        M_Card.Update(card, cnn, sqlTransaction);
                        e.NewValues.Add(nameof(card.CARD_ID), card.CARD_ID);
                        Log.Insert(Log.LogType.UPDATE, "Update card", JObject.FromObject(e.NewValues), cnn, sqlTransaction);
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

            var linqExpression = CriteriaToQueryableExtender.AppendWhere(new CMSContext().M_CARD, new CriteriaToEFExpressionConverter(), op);

            int takeData = 1000000;
            int rowCount = grid.VisibleRowCount;
            double c = Convert.ToDouble(rowCount / 1000000.00);
            var divideSheet = Math.Ceiling(c);

            List<M_CARD> data = new List<M_CARD>();
            var users = M_User.SelectAll().AsEnumerable();

            int i = 0;
            int dataCount = linqExpression.Count();
            foreach (M_CARD item in linqExpression)
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
                workSheet.Cells[1, 1].Value = "NIK";
                workSheet.Cells[1, 2].Value = "Card UID";
                workSheet.Cells[1, 3].Value = "COMMIT DATE";
                workSheet.Cells[1, 4].Value = "PROVINSI CODE";
                workSheet.Cells[1, 5].Value = "KABUPATEN/KOTA CODE";
                workSheet.Cells[1, 6].Value = "INNER";
                workSheet.Cells[1, 7].Value = "OUTER";
                workSheet.Cells[1, 8].Value = "Created By";
                workSheet.Cells[1, 9].Value = "Create Date";
                workSheet.Cells[1, 10].Value = "Update By";
                workSheet.Cells[1, 11].Value = "Update Date";

                //Body of table  
                int recordIndex = 2;
                i = 1;
                foreach (var field in data)
                {
                    try
                    {
                        var createby = users.Where(x => x.Field<int>("user_id") == field.CREATEBY).Select(x => x.Field<string>("fullname")).FirstOrDefault();
                        var updateby = users.Where(x => x.Field<int>("user_id") == field.UPDATEBY).Select(x => x.Field<string>("fullname")).FirstOrDefault();

                        string CREATEDATE = (field.CREATEDATE.HasValue) ? field.CREATEDATE.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
                        string UPDATEDATE = (field.UPDATEDATE.HasValue) ? field.UPDATEDATE.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
                        string CARDUID = (!string.IsNullOrEmpty(field.CARDUID)) ? field.CARDUID : string.Empty;
                        string NIK = (!string.IsNullOrEmpty(field.NIK)) ? field.NIK : string.Empty;
                        string CREATEBY = createby;
                        string UPDATEBY = updateby;
                        string COMMIT_DATE = field.COMMIT_DATE;
                        string PROVINSI_CODE = field.PROVINSI_CODE;
                        string KABKOTA_CODE = field.KABKOTA_CODE; 
                        string INNER = field.INNER; 
                        string OUTER = field.OUTER;

                        workSheet.Cells[recordIndex, 1].Value = NIK;
                        workSheet.Cells[recordIndex, 2].Value = CARDUID;
                        workSheet.Cells[recordIndex, 3].Value = COMMIT_DATE;
                        workSheet.Cells[recordIndex, 4].Value = PROVINSI_CODE;
                        workSheet.Cells[recordIndex, 5].Value = KABKOTA_CODE;
                        workSheet.Cells[recordIndex, 6].Value = INNER;
                        workSheet.Cells[recordIndex, 7].Value = OUTER;
                        workSheet.Cells[recordIndex, 8].Value = createby;
                        workSheet.Cells[recordIndex, 9].Value = CREATEDATE;
                        workSheet.Cells[recordIndex, 10].Value = updateby;
                        workSheet.Cells[recordIndex, 11].Value = UPDATEDATE;

                        string percent = (Math.Round((i * .001f) / (data.Count() * .001f) * 100.0f, 1)).ToString();
                        hub.Clients.User(username).getProgress("Generating excel data " + i + " of " + data.Count() + "(" + percent + "% progress). Elapsed time : " + stopwatch.Elapsed);
                        
                        NIK = null;
                        CARDUID = null;
                        COMMIT_DATE = null;
                        PROVINSI_CODE = null;
                        KABKOTA_CODE = null;
                        INNER = null;
                        OUTER = null;
                        CREATEBY = null;
                        CREATEDATE = null;
                        UPDATEBY = null;
                        UPDATEDATE = null;
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
                workSheet.Column(6).AutoFit();
                workSheet.Column(7).AutoFit();
                workSheet.Column(8).AutoFit();
                workSheet.Column(9).AutoFit();
                workSheet.Column(10).AutoFit();
                workSheet.Column(11).AutoFit();

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
                IEnumerable<M_CARD> itemFilter;

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
                    workSheet.Cells[1, 1].Value = "NIK";
                    workSheet.Cells[1, 2].Value = "Card UID";
                    workSheet.Cells[1, 3].Value = "COMMIT DATE";
                    workSheet.Cells[1, 4].Value = "PROVINSI CODE";
                    workSheet.Cells[1, 5].Value = "KABUPATEN/KOTA CODE";
                    workSheet.Cells[1, 6].Value = "INNER";
                    workSheet.Cells[1, 7].Value = "OUTER";
                    workSheet.Cells[1, 8].Value = "Created By";
                    workSheet.Cells[1, 9].Value = "Create Date";
                    workSheet.Cells[1, 10].Value = "Update By";
                    workSheet.Cells[1, 11].Value = "Update Date";

                    //Body of table  
                    int recordIndex = 2;
                    int filterCount = itemFilter.Count();
                    i = 0;
                    foreach (var field in itemFilter)
                    {
                        try
                        {
                            var createby = users.Where(x => x.Field<int>("user_id") == field.CREATEBY).Select(x => x.Field<string>("fullname")).FirstOrDefault();
                            var updateby = users.Where(x => x.Field<int>("user_id") == field.UPDATEBY).Select(x => x.Field<string>("fullname")).FirstOrDefault();

                            string CREATEDATE = (field.CREATEDATE.HasValue) ? field.CREATEDATE.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
                            string UPDATEDATE = (field.UPDATEDATE.HasValue) ? field.UPDATEDATE.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
                            string CARDUID = (!string.IsNullOrEmpty(field.CARDUID)) ? field.CARDUID : string.Empty;
                            string NIK = (!string.IsNullOrEmpty(field.NIK)) ? field.NIK : string.Empty;
                            string CREATEBY = createby;
                            string UPDATEBY = updateby;
                            string COMMIT_DATE = field.COMMIT_DATE;
                            string PROVINSI_CODE = field.PROVINSI_CODE;
                            string KABKOTA_CODE = field.KABKOTA_CODE;
                            string INNER = field.INNER;
                            string OUTER = field.OUTER;

                            workSheet.Cells[recordIndex, 1].Value = NIK;
                            workSheet.Cells[recordIndex, 2].Value = CARDUID;
                            workSheet.Cells[recordIndex, 3].Value = COMMIT_DATE;
                            workSheet.Cells[recordIndex, 4].Value = PROVINSI_CODE;
                            workSheet.Cells[recordIndex, 5].Value = KABKOTA_CODE;
                            workSheet.Cells[recordIndex, 6].Value = INNER;
                            workSheet.Cells[recordIndex, 7].Value = OUTER;
                            workSheet.Cells[recordIndex, 8].Value = createby;
                            workSheet.Cells[recordIndex, 9].Value = CREATEDATE;
                            workSheet.Cells[recordIndex, 10].Value = updateby;
                            workSheet.Cells[recordIndex, 11].Value = UPDATEDATE;

                            string percent = (Math.Round((i * .001f) / (filterCount * .001f) * 100.0f, 1)).ToString();
                            hub.Clients.User(username).getProgress("Generating sheet data " + sheet + " of " + divideSheet + ". Row " + i + " of " + filterCount + " (" + percent + "% progress). Elapsed time : " + stopwatch.Elapsed);

                            NIK = null;
                            CARDUID = null;
                            COMMIT_DATE = null;
                            PROVINSI_CODE = null;
                            KABKOTA_CODE = null;
                            INNER = null;
                            OUTER = null;
                            CREATEBY = null;
                            CREATEDATE = null;
                            UPDATEBY = null;
                            UPDATEDATE = null;
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
                    workSheet.Column(6).Width = 20;
                    workSheet.Column(7).Width = 20;
                    workSheet.Column(8).Width = 22;
                    workSheet.Column(9).Width = 14;
                    workSheet.Column(10).Width = 14;
                    workSheet.Column(11).Width = 30;

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

            List<M_Card> cards = new List<M_Card>();
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

                    var path = Server.MapPath("~/Uploads/") + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                    CSVUpload.SaveAs(path);
                    using (GenericParser parser = new GenericParser(path))
                    {
                        parser.ColumnDelimiter = Convert.ToChar(";");
                        parser.FirstRowHasHeader = true;
                        while (parser.Read())
                        {
                            try
                            {
                                var card = new M_Card();
                                card.NIK = parser["NIK"].ToString().Trim();
                                card.CARDUID = parser["CARDUID"].ToString().Trim();
                                card.COMMIT_DATE = parser["COMMIT_DATE"].ToString().Trim();
                                card.PROVINSI_CODE = parser["PROVINSI_CODE"].ToString().Trim();
                                card.KABKOTA_CODE = parser["KABKOTA_CODE"].ToString().Trim();
                                card.INNER = parser["INNER"].ToString().Trim();
                                card.OUTER = parser["OUTER"].ToString().Trim();
                                try
                                {
                                    string createdate = parser["CREATEDATE"].ToNullString();
                                    string updatedate = parser["UPDATEDATE"].ToNullString();

                                    if (!string.IsNullOrEmpty(createdate))
                                    {
                                        card.CREATEDATE = parser["CREATEDATE"].ToString().Trim();
                                    }
                                    else
                                    {
                                        card.CREATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    }

                                    if (!string.IsNullOrEmpty(updatedate))
                                    {
                                        card.UPDATEDATE = parser["UPDATEDATE"].ToString().Trim();
                                    }
                                    else
                                    {
                                        card.UPDATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    }
                                }
                                catch
                                {
                                    card.CREATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    card.UPDATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                

                                cards.Add(card);
                            }
                            catch
                            {
                                var card = new M_Card();
                                card.NIK = parser[0].ToString().Trim();
                                card.CARDUID = parser[1].ToString().Trim();
                                card.COMMIT_DATE = parser[2].ToString().Trim();
                                card.PROVINSI_CODE = parser[3].ToString().Trim();
                                card.KABKOTA_CODE = parser[4].ToString().Trim();
                                card.INNER = parser[5].ToString().Trim();
                                card.OUTER = parser[6].ToString().Trim();

                                try
                                {
                                    string createdate = parser[7].ToNullString();
                                    string updatedate = parser[8].ToNullString();
                                    if (!string.IsNullOrEmpty(createdate))
                                    {
                                        card.CREATEDATE = parser[7].ToString().Trim();
                                    }
                                    else
                                    {
                                        card.CREATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    }

                                    if (!string.IsNullOrEmpty(updatedate))
                                    {
                                        card.UPDATEDATE = parser[8].ToString().Trim();
                                    }
                                    else
                                    {
                                        card.UPDATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    }
                                }
                                catch
                                {
                                    card.CREATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    card.UPDATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                
                                cards.Add(card);
                            }
                        }

                        BulkInsert(cards, hub, username);
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

        public void BulkInsert(List<M_Card> listData, IHubContext hub, string username)
        {
            int countData = listData.Count();

            string table = "M_CARD";

            using (OracleConnection connection = new OracleConnection(Database.getConnectionString("Default")))
            {
                connection.Open();
                connection.CreateCommand();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    string[] _NIK = new string[countData];
                    string[] _CARDUID = new string[countData];
                    string[] _COMMIT_DATE = new string[countData];
                    string[] _PROVINSI_CODE = new string[countData];
                    string[] _KABKOTA_CODE = new string[countData];
                    string[] _INNER = new string[countData];
                    string[] _OUTER = new string[countData];
                    DateTime[] _CREATEDATE = new DateTime[countData];
                    DateTime[] _UPDATEDATE = new DateTime[countData];

                    for (int j = 0; j < countData; j++)
                    {
                        _NIK[j] = Convert.ToString(listData[j].NIK.ToString());
                        _CARDUID[j] = Convert.ToString(listData[j].CARDUID.ToString());
                        _COMMIT_DATE[j] = Convert.ToString(listData[j].COMMIT_DATE.ToString());
                        _PROVINSI_CODE[j] = Convert.ToString(listData[j].PROVINSI_CODE.ToString());
                        _KABKOTA_CODE[j] = Convert.ToString(listData[j].KABKOTA_CODE.ToString());
                        _INNER[j] = Convert.ToString(listData[j].INNER.ToString());
                        _OUTER[j] = Convert.ToString(listData[j].OUTER.ToString());
                        _CREATEDATE[j] = Convert.ToDateTime(listData[j].CREATEDATE);
                        _UPDATEDATE[j] = Convert.ToDateTime(listData[j].UPDATEDATE);
                    }

                    hub.Clients.User(username).getProgress($"Preparing object data...");

                    OracleParameter _NIK_PARAM = new OracleParameter();
                    _NIK_PARAM.OracleDbType = OracleDbType.Varchar2;
                    _NIK_PARAM.Value = _NIK;

                    OracleParameter _CARDUID_PARAM = new OracleParameter();
                    _CARDUID_PARAM.OracleDbType = OracleDbType.Varchar2;
                    _CARDUID_PARAM.Value = _CARDUID;

                    OracleParameter _COMMIT_DATE_PARAM = new OracleParameter();
                    _COMMIT_DATE_PARAM.OracleDbType = OracleDbType.Varchar2;
                    _COMMIT_DATE_PARAM.Value = _COMMIT_DATE;

                    OracleParameter _PROVINSI_CODE_PARAM = new OracleParameter();
                    _PROVINSI_CODE_PARAM.OracleDbType = OracleDbType.Varchar2;
                    _PROVINSI_CODE_PARAM.Value = _PROVINSI_CODE;

                    OracleParameter _KABKOTA_CODE_PARAM = new OracleParameter();
                    _KABKOTA_CODE_PARAM.OracleDbType = OracleDbType.Varchar2;
                    _KABKOTA_CODE_PARAM.Value = _KABKOTA_CODE;

                    OracleParameter _INNER_PARAM = new OracleParameter();
                    _INNER_PARAM.OracleDbType = OracleDbType.Varchar2;
                    _INNER_PARAM.Value = _INNER;

                    OracleParameter _OUTER_PARAM = new OracleParameter();
                    _OUTER_PARAM.OracleDbType = OracleDbType.Varchar2;
                    _OUTER_PARAM.Value = _OUTER;

                    OracleParameter _CREATEDATE_PARAM = new OracleParameter();
                    _CREATEDATE_PARAM.OracleDbType = OracleDbType.Date;
                    _CREATEDATE_PARAM.Value = _CREATEDATE;

                    OracleParameter _UPDATEDATE_PARAM = new OracleParameter();
                    _UPDATEDATE_PARAM.OracleDbType = OracleDbType.Date;
                    _UPDATEDATE_PARAM.Value = _UPDATEDATE;
                    

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
                                NIK,
                                CARDUID,
                                COMMIT_DATE,
                                PROVINSI_CODE,
                                KABKOTA_CODE,
                                INNER,
                                OUTER,
                                CREATEDATE,
                                UPDATEDATE,
                                CREATEBY,
                                UPDATEBY
                            )
                            VALUES
                            (
                                :1,
                                :2,
                                :3,
                                :4,
                                :5,
                                :6,
                                :7,
                                :8,
                                :9,
                                {1},
                                {1}
                            )
                            ", table, M_User.getUserId());

                            command.CommandText = insert;
                            command.ArrayBindCount = countData;

                            command.Parameters.Clear();
                            command.Parameters.Add(_NIK_PARAM);
                            command.Parameters.Add(_CARDUID_PARAM);
                            command.Parameters.Add(_COMMIT_DATE_PARAM);
                            command.Parameters.Add(_PROVINSI_CODE_PARAM);
                            command.Parameters.Add(_KABKOTA_CODE_PARAM);
                            command.Parameters.Add(_INNER_PARAM);
                            command.Parameters.Add(_OUTER_PARAM);
                            command.Parameters.Add(_CREATEDATE_PARAM);
                            command.Parameters.Add(_UPDATEDATE_PARAM);

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

                    _NIK_PARAM = null;
                    _CARDUID_PARAM = null;
                    _COMMIT_DATE_PARAM = null;
                    _PROVINSI_CODE_PARAM = null;
                    _KABKOTA_CODE_PARAM = null;
                    _INNER_PARAM = null;
                    _OUTER_PARAM = null;
                    _CREATEDATE_PARAM = null;
                    _UPDATEDATE_PARAM = null;

                    _NIK = null;
                    _CARDUID = null;
                    _COMMIT_DATE = null;
                    _PROVINSI_CODE = null;
                    _KABKOTA_CODE = null;
                    _INNER = null;
                    _OUTER = null;
                    _CREATEDATE = null;
                    _UPDATEDATE = null;
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