using CMS.API.Class;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace CMS
{
    public class GlobalExceptionMiddleware : OwinMiddleware
    {
        public GlobalExceptionMiddleware(OwinMiddleware next) : base(next)
        { }

        public override async Task Invoke(IOwinContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (AuthenticationError ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                // your handling logic
            }
        }

        private Task HandleExceptionAsync(IOwinContext context, Exception exception)
        {
            string result = new 
            {
                message = exception.Message,
                code = (int)HttpStatusCode.InternalServerError
            }.ToString();

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return context.Response.WriteAsync(result);
        }
    }
}