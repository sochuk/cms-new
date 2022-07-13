using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CMS.Helper
{
    public static class Strings
    {
        public static string Append(this string str, string s) => str + s;

        public static string Prepend(this string str, string s) => s + str;

        public static string AppendPrepend(this string str, string s) => s + str + s;

        private static string GetMD5Hash(string data)
        {
            return String.Join("", MD5.Create()
                         .ComputeHash(Encoding.Default.GetBytes(data))
                         .Select(b => b.ToString("x2")));
        }
    }
}