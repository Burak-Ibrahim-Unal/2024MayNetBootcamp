using Bootcamp.Service.ProductService.Configurations;
using Bootcamp.Service;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.Api.Extensions
{
    public static class ServiceExt
    {
        public static void AddService(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(x => { x.SuppressModelStateInvalidFilter = true; });
            services.AddAutoMapper(typeof(ServiceAssembly).Assembly);
            services.AddFluentValidationAutoValidation();
            services.AddProductService();
        }
    }
}
