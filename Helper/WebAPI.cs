using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CMS.Helper
{
    public static class WebAPI
    {
        public static HttpStatusCode getToken(this HttpRequestMessage requestMessage)
        {
            IEnumerable<string> headerValues;
            if (requestMessage.Headers.TryGetValues("token", out headerValues))
            {
                if (headerValues.FirstOrDefault() == "123456")
                {
                    return HttpStatusCode.OK;
                }
            }
            return HttpStatusCode.BadRequest;
        }

        public static async Task<string> GetAuthorizeToken()
        {
            // Initialization.  
            string responseObj = string.Empty;

            // Posting.  
            using (var client = new HttpClient())
            {
                var res = client.PostAsync(Application.GetHost()+ "api/user/login",
                     new StringContent(JsonConvert.SerializeObject(
                       new
                       {
                           username = "super",
                           password = "P@ssW0rd!"
                       }),
                       Encoding.UTF8, "application/json")
                   );

                try
                {
                    res.Result.EnsureSuccessStatusCode();
                    responseObj = res.Result.Content.ReadAsStringAsync().Result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

               
            }

            return responseObj;
        }


    }
}