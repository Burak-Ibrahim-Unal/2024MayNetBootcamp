using Microsoft.AspNetCore.Http;
using System.Net;

namespace Bootcamp.Service.ExceptionHandlers
{
    public class IPWhiteListMiddleware(RequestDelegate next)
    {
        private List<IPAddress> whiteList = [IPAddress.Parse("::1"), IPAddress.Parse("127.0.0.1")];
        public async Task InvokeAsync(HttpContext context)
        {
            //check swagger
            if (context.Request.Path.Value.Contains("swagger"))
            {
                await next(context);
                return;
            }

            var ip = context.Connection.RemoteIpAddress;

            if (!whiteList.Contains(ip))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Not Authorized");
                return;
            }

            await next(context);
        }
    }
}
