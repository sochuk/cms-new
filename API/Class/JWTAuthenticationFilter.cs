using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CMS.API.Class
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class JWTAuthenticationFilter : AuthorizationFilterAttribute, IAuthorizationFilter
    {
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (!isAuthorized(filterContext))
            {
                var response = new { code = 401, message = "Invalid access, please login first" };
                filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized, response);
                throw new ApiException(HttpStatusCode.Unauthorized, JsonConvert.SerializeObject(response));
            }
            base.OnAuthorization(filterContext);
        }

        private void AuthenticationError (HttpActionContext filterContext)
        {
            var response = new { code = 401, message = "Invalid access, please login first" };
            filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized, response);
            filterContext.Response.StatusCode = HttpStatusCode.Unauthorized;
        }

        private bool isAuthorized(HttpActionContext filterContext)
        {
            string token = null;
            if (filterContext.Request.Headers.Contains("Authorization"))
            {
                token = filterContext.Request.Headers.GetValues("Authorization").FirstOrDefault();
            }

            if (filterContext.Request.Headers.Contains("token"))
            {
                token = filterContext.Request.Headers.GetValues("token").FirstOrDefault();
            }

            if (token == null)
            {
                return false;
            }
            else
            {
                token = token.Replace("Bearer", "").Trim();
                var key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["key"].ToString());
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                try
                {
                    SecurityToken validationsResult;
                    var result = tokenHandler.ValidateToken(token, tokenValidationParameters, out validationsResult);
                    if (result != null)
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }
    }
}