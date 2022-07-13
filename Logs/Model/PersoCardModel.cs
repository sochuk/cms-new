using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Logs.Model
{
    public class PersoCardModel
    {
        public int? PersoStep { get; set; }
        public string PersoType { get; set; }
        public byte[] FileData { get; set; }
        public string ControlNumber { get; set; }
        public string ManufacturerCode { get; set; }
        public string PersoSite { get; set; }
        public string CardUID { get; set; }
        public string RandomNumber { get; set; }
        public string PersoScript { get; set; }
        public string BioData { get; set; }
        public string Photograph { get; set; }
        public string SignatureScan { get; set; }
        public int TransportKey { get; set; }
        public string ErrorCode { get; set; }
    }
}