using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace CMS.Helper
{
    public class IPAddress
    {
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return string.Empty;
        }

        public static string GetRemoteIPAddress()
        {
            var context = HttpContext.Current;
            if (context.Request.Headers["CF-CONNECTING-IP"] != null) return context.Request.Headers["CF-CONNECTING-IP"].ToString();

            if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!string.IsNullOrEmpty(ipAddress))
                {
                    string[] addresses = ipAddress.Split(',');
                    if (addresses.Length != 0)
                    {
                        return addresses[0];
                    }
                }
            }

            return context.Request.UserHostAddress;
        }
    }
}