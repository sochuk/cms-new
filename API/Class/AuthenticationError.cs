using Newtonsoft.Json;
using System;
using System.Net;
using System.Runtime.Serialization;
using System.Web.Http.Controllers;

namespace CMS.API.Class
{
    [Serializable]
    internal class AuthenticationError : Exception
    {
        private HttpActionContext filterContext;
        public HttpStatusCode StatusCode { get; set; }
        public string ContentType { get; set; } = @"text/plain";

        public AuthenticationError()
        {
        }

        public AuthenticationError(HttpActionContext filterContext)
        {
            this.filterContext = filterContext;
        }

        public AuthenticationError(string message) : base(message)
        {
        }

        public AuthenticationError(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AuthenticationError(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}