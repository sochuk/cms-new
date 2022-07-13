using CMS.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CMS.Master.Model
{
    public class M_Dashboard_Summary
    {
        public string TITLE { get; set; }
        public int? VALUE { get; set; }  
        public string DESCRIPTION { get; set; } 


        public M_Dashboard_Summary GetTotalMassal()
        {
            M_Dashboard_Summary summary = new M_Dashboard_Summary();

            DataTable data = Database.getDataTable("select count(1) from M_CARD where comments = 'MASSAL'");

            summary.TITLE = "Total Pencetakan Blangko & KTP-el Massal (2011-2014)";
            summary.VALUE = data.Rows.Count > 0 ? data.Rows[0]["COUNT"].ToInteger() : 0;
            summary.DESCRIPTION = "Total Pencetakan Blangko & KTP-el Massal (2011-2014)";

            return summary;

        }

        public M_Dashboard_Summary GetTotalPIAK()
        {
            M_Dashboard_Summary summary = new M_Dashboard_Summary();

            DataTable data = Database.getDataTable("select count(1) from M_CARD where nik is not null OR CARUID is not null or nama is not null or createdate is not null");

            summary.TITLE = "Total Pencetakan KTP-el Reguler (2014 - Sekarang)";
            summary.VALUE = data.Rows.Count > 0 ? data.Rows[0]["COUNT"].ToInteger() : 0;
            summary.DESCRIPTION = "Total Pencetakan KTP-el Reguler (2014 - Sekarang)";

            return summary;

        }

        public M_Dashboard_Summary GetTotalCardPrinted()
        {
            M_Dashboard_Summary summary = new M_Dashboard_Summary();

            DataTable data = Database.getDataTable("select count(1) from KMS.card  where nik is not null");

            summary.TITLE = "Total Pencetakan KTP-el Reguler (2014 - Sekarang)";
            summary.VALUE = data.Rows.Count > 0 ? data.Rows[0]["COUNT"].ToInteger() : 0;
            summary.DESCRIPTION = "Total Pencetakan KTP-el Reguler (2014 - Sekarang)";

            return summary;

        }
    }
}