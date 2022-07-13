using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CMS.Hubs
{
    public class HubUser
    {
        public string connection_id { get; set; }

        public string user_id { get; set; } // Encrypt with Crypto.Encrypt. Passphare "message"
        public string username { get; set; } // Encrypt with Crypto.Encrypt. Passphare "message"
        public string fullname { get; set; }
        public string company_name { get; set; }
        public string gender { get; set; }
        public string photo { get; set; }
        public string local_ipaddress { get; set; }
        public string remote_ipaddress { get; set; }
        public string user_agent { get; set; }
    }
}