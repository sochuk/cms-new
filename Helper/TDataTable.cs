using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CMS.Helper
{
    public class TDataTable : DataTable
    {
        private string name { get; set; }
        private DataTable data { get; set; }

        public TDataTable(string name)
        {
            this.name = name;
            this.data = new DataTable();
            HttpContext.Current.Session[name] = this.data;
        }

        public TDataTable(string name, DataTable data)
        {
            HttpContext.Current.Session[name] = data;
        }

        public void Add(DataTable data)
        {
            this.data = data;
            HttpContext.Current.Session[name] = this.data;
        }

        public DataTable Load()
        {
            return HttpContext.Current.Session[this.name] as DataTable;
        }

        public void Clear()
        {
            HttpContext.Current.Session[this.name] = null;
        }

        public static void Add(string name, DataTable data)
        {
            HttpContext.Current.Session[name] = data;
        }

        public static DataTable Load(string name)
        {
            return HttpContext.Current.Session[name] as DataTable;
        }

        public static void Clear(string name)
        {
            HttpContext.Current.Session[name] = null;
        }

        protected override void OnRowChanged(DataRowChangeEventArgs e)
        {
            base.OnRowChanged(e);
        }
    }
}