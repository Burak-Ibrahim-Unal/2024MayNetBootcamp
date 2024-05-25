using Bootcamp.Service.SharedDto;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Bootcamp.Service.ExceptionHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var responseModel = ResponseModelDto<NoContent>.Fail(exception.Message, HttpStatusCode.InternalServerError);
            await httpContext.Response.WriteAsJsonAsync(responseModel, cancellationToken: cancellationToken);
            //var result = ValueTask.FromResult(true);
            return true;
        }
    }
}
