using CMS.Helper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace CMS.Management.Model
{
    public class M_Card
    {
        public enum CardStatus
        {
            Valid,
            Invalid,
            NotFound
        }

        public int CARD_ID { get; set; }
        public string NIK { get; set; }
        public string CARDUID { get; set; }
        public string PROVINSI_CODE { get; set; }
        public string KABKOTA_CODE { get; set; }
        public string INNER { get; set; }
        public string OUTER { get; set; }
        public int? CREATEBY { get; set; }
        public int? UPDATEBY { get; set; }
        public string CREATEDATE { get; set; }
        public string UPDATEDATE { get; set; }
        public string COMMIT_DATE { get; set; }

        public static M_Card Insert(M_Card card, OracleConnection connection, OracleTransaction transaction)
        {
            var id = Database.executeScalar("CARD_ID",
                @"INSERT INTO M_CARD(NIK, CARDUID, COMMIT_DATE, PROVINSI_CODE, KABKOTA_CODE, INNER, OUTER, CREATEBY, CREATEDATE, UPDATEBY, UPDATEDATE)
                VALUES(:NIK, :CARDUID, :COMMIT_DATE, :PROVINSI_CODE, :KABKOTA_CODE, :INNER, :OUTER, :CREATEBY, SYSDATE, :UPDATEBY, SYSDATE)", connection, transaction,
                new OracleParameter(":NIK", card.NIK),
                new OracleParameter(":CARDUID", card.CARDUID),
                new OracleParameter(":COMMIT_DATE", card.COMMIT_DATE),
                new OracleParameter(":PROVINSI_CODE", card.PROVINSI_CODE),
                new OracleParameter(":KABKOTA_CODE", card.KABKOTA_CODE),
                new OracleParameter(":INNER", card.INNER),
                new OracleParameter(":OUTER", card.OUTER),
                new OracleParameter(":CREATEBY", card.CREATEBY ?? M_User.getUserId()),
                new OracleParameter(":UPDATEBY", card.UPDATEBY ??  M_User.getUserId())
                );

            card.CARD_ID = id.ToInteger();
            return card;
        }

        public static CardStatus Validate(M_Card card, OracleConnection connection, OracleTransaction transaction, out string lastUpdate)
        {
            lastUpdate = null;
            DataTable dt = Database.getDataTable(
                @"SELECT * FROM M_CARD WHERE NIK=:NIK ORDER BY CREATEDATE DESC", 
                connection, transaction,
                new OracleParameter(":NIK", card.NIK)
                );

            if(dt.Rows.Count > 0)
            {
                if(dt.Rows[0]["CARDUID"].ToString() == card.CARDUID)
                {
                    lastUpdate = ((DateTime)dt.Rows[0]["CREATEDATE"]).ToString("yyyy-MM-dd HH:mm:ss");
                    return CardStatus.Valid;
                }
            }
            else
            {
                return CardStatus.NotFound;
            }

            return CardStatus.Invalid;
        }

        public static M_Card Update(M_Card group, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE M_CARD 
                SET NIK=:NIK, 
                CARDUID=:CARDUID,
                COMMIT_DATE=:COMMIT_DATE,
                PROVINSI_CODE=:PROVINSI_CODE,
                KABKOTA_CODE=:KABKOTA_CODE,
                INNER=:INNER,
                OUTER=:OUTER,
                UPDATEBY=:UPDATEBY,
                UPDATEDATE=SYSDATE
                WHERE CARD_ID=:CARD_ID",
                connection, transaction,
                new OracleParameter(":CARD_ID", group.CARD_ID),
                new OracleParameter(":NIK", group.NIK),
                new OracleParameter(":CARDUID", group.CARDUID),
                new OracleParameter(":COMMIT_DATE", group.COMMIT_DATE),
                new OracleParameter(":PROVINSI_CODE", group.PROVINSI_CODE),
                new OracleParameter(":KABKOTA_CODE", group.KABKOTA_CODE),
                new OracleParameter(":INNER", group.INNER),
                new OracleParameter(":OUTER", group.OUTER),
                new OracleParameter(":UPDATEBY", M_User.getUserId())
                );

            return group;
        }

        public static bool IsExist(M_Card data)
        {
            DataTable dt = Database.getDataTable("SELECT * FROM M_CARD where NIK=:NIK",
                new OracleParameter(":NIK", data.NIK));

            if (dt.Rows.Count > 0) return true;

            return false;
        }

        public static int Total()
        {
            DataTable dt = Database.getDataTable("SELECT COUNT(*) COUNT FROM M_CARD");

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["COUNT"].ToInteger();
            }

            return 0;
        }

        public static int Duplicate()
        {
            DataTable dt = Database.getDataTable(@"SELECT COUNT(*) COUNT FROM (
                SELECT NIK, COUNT(NIK) C FROM M_CARD GROUP BY NIK) A
                WHERE A.C > 1");

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["COUNT"].ToInteger();
            }

            return 0;
        }

        public static DataTable SelectAll()
        {
            string sql = string.Format(@"
                    SELECT * FROM M_CARD", M_User.getDatabaseName());
            DataTable dt = Database.getDataTable(sql);

            List<M_Card> data = new List<M_Card>();
            foreach (DataRow row in dt.Rows)
            {
                data.Add(
                    new M_Card()
                    {
                        CARD_ID = row["CARD_ID"].ToInteger(),
                        NIK = row["NIK"].ToString(),
                        CARDUID = row["CARDUID"].ToString(),
                        COMMIT_DATE = row["COMMIT_DATE"].ToString(),
                        PROVINSI_CODE = row["PROVINSI_CODE"].ToString(),
                        KABKOTA_CODE = row["KABKOTA_CODE"].ToString(),
                        INNER = row["INNER"].ToString(),
                        OUTER = row["OUTER"].ToString(),
                        CREATEBY = row["CREATEBY"].ToInteger(),
                        UPDATEBY = row["UPDATEBY"].ToInteger(),
                        CREATEDATE = row["CREATEDATE"].ToString(),
                        UPDATEDATE = row["UPDATEDATE"].ToString(),
                    }
                );
            }

            DataTable mdt = data.ToDataTable();

            return mdt;
        }
    }
}