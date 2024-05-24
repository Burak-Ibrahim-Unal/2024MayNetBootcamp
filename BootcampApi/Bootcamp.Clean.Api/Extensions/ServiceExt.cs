using Bootcamp.Clean.ApplicationService;
using Bootcamp.Clean.ApplicationService.ProductService.Configurations;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.Clean.Api.Extensions
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
