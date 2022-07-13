using CMS.Context;
using CMS.Helper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Master.Model
{
    public class M_Provinsi
    {
        public int ID { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }

        public static M_Provinsi Insert(M_Provinsi provinsi, OracleConnection connection, OracleTransaction transaction)
        {
            var mProvinsiList = new List<M_PROVINSI>();

            var m_PROVINSI = mProvinsiList.LastOrDefault();
            var lastIdProv = m_PROVINSI.ID;
            var newIdProv = lastIdProv + 1;

             Database.querySQL(@"INSERT INTO M_PROVINSI(ID, CODE, NAME)
                VALUES(:ID, :CODE, :NAME)", connection, transaction,
                new OracleParameter(":ID", newIdProv),
                new OracleParameter(":CODE", provinsi.CODE),
                new OracleParameter(":NAME", provinsi.NAME)
                );

            Console.WriteLine(newIdProv);
            provinsi.ID = newIdProv;
            return provinsi;
        }


        public static M_Provinsi Update(M_Provinsi group, OracleConnection connection, OracleTransaction transaction)
        {
            Database.querySQL(@"
                UPDATE M_PROVINSI 
                SET CODE=:CODE, 
                NAME=:NAME
                WHERE ID=:ID",
                connection, transaction,
                new OracleParameter(":CODE", group.CODE),
                new OracleParameter(":NAME", group.NAME)
                );

            return group;
        }

    }
}
