using CMS.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CMS.Notification
{
    public class SMS
    {
        private static async Task<bool> Send(string To, string Message)
        {
            using (HttpClient client = new HttpClient(new HttpClientHandler()))
            {
                To = To.Replace("-", string.Empty);
                To = To.Replace("+", string.Empty);
                string host = ConfigurationManager.AppSettings.Get("SMS_Server").ToString();
                var content = new List<KeyValuePair<string, string>>
                {
                     new KeyValuePair<string, string>("user", "DUKCAPIL"),
                     new KeyValuePair<string, string>("password", "duk123"),
                     new KeyValuePair<string, string>("senderid ", "DUKCAPIL"),
                     new KeyValuePair<string, string>("message ", Message),
                     new KeyValuePair<string, string>("msisdn ", To),
                };
                HttpResponseMessage response = await client.PostAsync(host, new FormUrlEncodedContent(content));
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                if (responseString.Contains("FAILED"))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static async Task<bool> SendTo(string Phone, string Message)
        {
            Phone = Phone.Replace("-", string.Empty);
            Phone = Phone.Replace("+", string.Empty);
            Phone = Phone.Replace(" ", string.Empty);

            using (WebClient webClient = new WebClient())
            {
                string host = ConfigurationManager.AppSettings.Get("sms_server").ToString();
                string user = ConfigurationManager.AppSettings.Get("sms_user").ToString();
                string key = ConfigurationManager.AppSettings.Get("key").ToString();
                string password = Crypto.Decrypt(ConfigurationManager.AppSettings.Get("sms_pwd").ToString(), key);
                string senderid = ConfigurationManager.AppSettings.Get("sms_senderid").ToString();
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                var parameters = new System.Collections.Specialized.NameValueCollection();
                parameters.Add("user", user);
                parameters.Add("password", password);
                parameters.Add("senderid", senderid);
                parameters.Add("message", Message);
                parameters.Add("msisdn", Phone);

                byte[] responsebytes = webClient.UploadValues(host, "POST", parameters);
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                if (responsebody.Contains("SUCCESS"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
    }
}