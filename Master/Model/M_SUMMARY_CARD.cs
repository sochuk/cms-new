using CMS.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CMS.Master.Model
{
    public class M_SUMMARY_CARD
    {
        public string TITLE { get; set; }
        public int VALUE { get; set; }  
        public string DESCRIPTION { get; set; }

        public M_SUMMARY_CARD GetTotalMassal()
        {
            M_SUMMARY_CARD result = new M_SUMMARY_CARD();

            DataTable dt = Database.getDataTable("select count(1) COUNT from card where comments = 'MASSAL'");


            result.TITLE = "Total Pencetakan Blangko & KTP-el Massal (2011-2014)";
            result.VALUE = dt.Rows.Count > 0 ? dt.Rows[0]["COUNT"].ToInteger() : 0;
            result.DESCRIPTION = "Total Pencetakan Blangko & KTP-el Massal (2011-2014)";

            return result;
        }

        public M_SUMMARY_CARD GetTotalPiak()
        {
            M_SUMMARY_CARD result = new M_SUMMARY_CARD();

            DataTable dt = Database.getDataTable("select count(1) from card where nik is not null OR CARUID is not null or nama is not null or createdate is not null");

            result.TITLE = "Total Pencetakan KTP-el Reguler (2014 - Sekarang)";
            result.VALUE = dt.Rows.Count > 0 ? dt.Rows[0]["COUNT"].ToInteger() : 0;
            result.DESCRIPTION = "Total Pencetakan KTP-el Reguler (2014 - Sekarang)";

            return result;
        }

        public M_SUMMARY_CARD GetTotalRegisteredCard()
        {
            M_SUMMARY_CARD result = new M_SUMMARY_CARD();

            DataTable dt = Database.getDataTable("select count(1) COUNT from KMS.CARD_CARDUID");

            result.TITLE = "Total NIK Yang Mencetak KTP-el (2011 - Sekarang)";
            result.VALUE = dt.Rows.Count > 0 ? dt.Rows[0]["COUNT"].ToInteger() : 0;
            result.DESCRIPTION = "Total NIK Yang Mencetak KTP-el (2011 - Sekarang)";

            return result;
        }

        public M_SUMMARY_CARD GetTotalRegisteredNIK()
        {
            M_SUMMARY_CARD result = new M_SUMMARY_CARD();

            DataTable dt = Database.getDataTable("select count(1) COUNT from kms.CARD_NIK");

            result.TITLE = "Total NIK Yang Mencetak KTP-el (2011 - Sekarang)";
            result.VALUE = dt.Rows.Count > 0 ? dt.Rows[0]["COUNT"].ToInteger() : 0;
            result.DESCRIPTION = "Total NIK Yang Mencetak KTP-el (2011 - Sekarang)";

            return result;
        }

        public M_SUMMARY_CARD GetTotalCardManufactured()
        {
            M_SUMMARY_CARD result = new M_SUMMARY_CARD();

            DataTable dt = Database.getDataTable("select count(card_carduid.carduid) COUNT from  KMS.card_carduid , KMS.log_perso where card_carduid.carduid = log_perso.carduid and log_perso.is_commit = 1");

            result.TITLE = "Total Pencetakan Blangko Reguler (2016 - Sekarang)";
            result.VALUE = dt.Rows.Count > 0 ? dt.Rows[0]["COUNT"].ToInteger() : 0;
            result.DESCRIPTION = "Total Pencetakan Blangko Reguler (2016 - Sekarang)";

            return result;
        }
    }
}