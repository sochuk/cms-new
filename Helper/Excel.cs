using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace CMS.Helper
{
    public static class Excel
    {
        public static DataTable ReadExcel(string FileName)
        {
            DataTable dt = new DataTable();

            //or if you use asp.net, get the relative path
            byte[] bin = File.ReadAllBytes(FileName);

            //create a new Excel package in a memorystream
            using (MemoryStream stream = new MemoryStream(bin))
            using (ExcelPackage excelPackage = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];

                dt.TableName = worksheet.Name;

                //check if the worksheet is completely empty
                if (worksheet.Dimension == null)
                {
                    return dt;
                }

                //create a list to hold the column names
                List<string> columnNames = new List<string>();

                //needed to keep track of empty column headers
                int currentColumn = 1;

                //loop all columns in the sheet and add them to the datatable
                foreach (var cell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                {
                    string columnName = cell.Text.Trim();

                    //check if the previous header was empty and add it if it was
                    if (cell.Start.Column != currentColumn)
                    {
                        columnNames.Add("Header_" + currentColumn);
                        dt.Columns.Add("Header_" + currentColumn);
                        currentColumn++;
                    }

                    //add the column name to the list to count the duplicates
                    columnNames.Add(columnName);

                    //count the duplicate column names and make them unique to avoid the exception
                    //A column named 'Name' already belongs to this DataTable
                    int occurrences = columnNames.Count(x => x.Equals(columnName));
                    if (occurrences > 1)
                    {
                        columnName = columnName + "_" + occurrences;
                    }

                    //add the column to the datatable
                    dt.Columns.Add(columnName);

                    currentColumn++;
                }

                //start adding the contents of the excel file to the datatable
                for (int i = 2; i <= worksheet.Dimension.End.Row; i++)
                {
                    var row = worksheet.Cells[i, 1, i, worksheet.Dimension.End.Column];
                    DataRow newRow = dt.NewRow();

                    //loop all cells in the row
                    foreach (var cell in row)
                    {
                        if (cell.Address.Substring(0, 1) == "A")
                        {
                            if (cell.Value == null) break;
                        }

                        try
                        {
                            if (cell.Value.GetType() == typeof(DateTime))
                            {
                                newRow[cell.Start.Column - 1] = (DateTime?)cell?.Value ?? (DateTime?)null;
                            }
                            else if (cell.Value.GetType() == typeof(Double))
                            {
                                newRow[cell.Start.Column - 1] = Convert.ToInt32((double)cell.Value);
                            }
                            else
                            {
                                newRow[cell.Start.Column - 1] = (string)cell.Text.ToString();
                            }
                        }
                        catch
                        {
                            newRow[cell.Start.Column - 1] = (string)cell.Text.ToString();
                        }

                    }

                    dt.Rows.Add(newRow);
                }

                return dt;
            }
        }
    }
}