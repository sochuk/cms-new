using CMS.Helper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Master.Model
{
    public class M_KabKota
    {
        public int ID { get; set; }
        public int PROVINSI_ID { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string TYPE { get; set; }

        public static M_KabKota Insert(M_KabKota kabKota, OracleConnection connection, OracleTransaction transaction)
        {

            var id = Database.executeScalar("ID",
                @"INSERT INTO M_KABKOTA(PROVINSI_ID, CODE, NAME, TYPE)
                VALUES(:CODE, :NAME)", connection, transaction,
                new OracleParameter(":PROVINSI_ID", kabKota.PROVINSI_ID),
                new OracleParameter(":CODE", kabKota.CODE),
                new OracleParameter(":NAME", kabKota.NAME),
                new OracleParameter(":TYPE", kabKota.TYPE)
                ); ; ;

            kabKota.ID = id.ToInteger();
            return kabKota;
        }

        public static M_KabKota Update(M_KabKota group, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE M_KABKOTA 
                SET PROVINSI_ID=:PROVINSI_ID, 
                CODE=:CODE
                NAME=:NAME
                TYPE=:TYPE
                WHERE ID=:ID",
                connection, transaction,
                new OracleParameter(":PROVINSI_ID", group.PROVINSI_ID),
                new OracleParameter(":CODE", group.CODE),
                new OracleParameter(":NAME", group.NAME),
                new OracleParameter(":TYPE", group.TYPE)
                );

            return group;
        }
    }
}