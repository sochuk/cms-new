using Newtonsoft.Json.Linq;
using CMS.Helper;
using CMS.Management.Model;
using System.Security.Claims;
using System.Web.Http;
using Oracle.ManagedDataAccess.Client;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using System.Collections.Generic;
using CMS.API.Class;
using System.Web;
using System.Net.Http;
using System;
using System.Net;

namespace CMS.API.Controllers
{
    public class UserController : APIController
    {
        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        [ActionName("login")]
        [HttpPost]
        public IHttpActionResult Login([FromBody] JObject json)
        {
            string username = null;
            string password = null;
            try
            {
                username = json.GetValue("username").Value<string>().Trim();
                password = json.GetValue("password").Value<string>().Trim();
            }
            catch
            {
                return Json(new
                {
                    code = HttpStatusCode.ExpectationFailed,
                    message = "Username or password required"
                });
            }

            password = Crypto.EncryptPassword(password);

            M_User user = new M_User();
            using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
            {
                cnn.Open();
                using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                {
                    var ip = GetIPAddress();
                    user = M_User.Login(username, password, ip, ip, "API", "API");
                }
            }

            if(user != null)
            {
                var key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["key"].ToString());
                var tokenHandler = new JwtSecurityTokenHandler();
                var claims = new List<Claim>(){
                        new Claim(UserClaim.User_Id, user.user_id.ToString()),
                        new Claim(UserClaim.Username, user.username),
                        new Claim(UserClaim.Last_Login, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                };

                var claimsIdentity = new ClaimsIdentity(claims);

                var securityTokenDescriptor = new SecurityTokenDescriptor
                {
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest),
                    Subject = claimsIdentity,
                    Expires = DateTime.Now.AddYears(1),
                };

                var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
                var signedAndEncodedToken = tokenHandler.WriteToken(plainToken);

                return Json(new
                {
                    code = HttpStatusCode.OK,
                    message = "Success",
                    data = new {
                        user_id = user.user_id,
                        username = user.username,
                        fullname = user.fullname,
                        token = signedAndEncodedToken
                    }
                   
                });
            }

            return Json(new
            {
                code = HttpStatusCode.ExpectationFailed,
                message = "Invalid username or password",
                data = new { }
            });

        }

        public IHttpActionResult Validate()
        {
            var user = new M_User();
            if (isAuthorized(out user))
            {
                return Json(new
                {
                    code = HttpStatusCode.OK,
                    message = "Bearer token is valid"
                });
            }

            return Content(HttpStatusCode.Forbidden, new
            {
                code = HttpStatusCode.Forbidden,
                status = "error",
                message = "Bearer token not valid"
            });

        }
    }
}
