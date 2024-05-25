using Bootcamp.Service.ExceptionHandlers;
using Bootcamp.Service.SharedDto;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace Bootcamp.Api.Extensions
{
    public static class MiddlewareExt
    {
        public static void AddMiddlewares(this WebApplication app)
        {
            app.UseExceptionHandler();
            #region Before Net 8
            //app.UseExceptionHandler(appBuilder =>
            //{
            //    appBuilder.Run(async context =>
            //    {
            //        //var loggerFactory= context.RequestServices.GetService<ILoggerFactory>();

            //        // var logger= loggerFactory!.CreateLogger("GlobalExceptionLogger");

            //        context.Response.StatusCode = 500;
            //        context.Response.ContentType = "application/json";
            //        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();


            //        if (contextFeature != null)
            //        {
            //            var exception = contextFeature.Error;

            //            var responseModel =
            //                ResponseModelDto<NoContent>.Fail(exception.Message, HttpStatusCode.InternalServerError);

            //            await context.Response.WriteAsJsonAsync(responseModel);
            //        }
            //    });
            //}); 
            #endregion

            app.UseMiddleware<IPWhiteListMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
