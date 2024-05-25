using Bootcamp.Service.ProductService.Configurations;
using Bootcamp.Service;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Bootcamp.Service.Weather;
using Bootcamp.Service.Token;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Bootcamp.Service.ExceptionHandlers;

namespace Bootcamp.Service.Extensions
{
    public static class ServiceExt
    {
        public static void AddService(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<ApiBehaviorOptions>(x => { x.SuppressModelStateInvalidFilter = true; });
            services.AddAutoMapper(typeof(ServiceAssembly).Assembly);
            services.AddFluentValidationAutoValidation();

            services.AddProductService();

            services.AddExceptionHandler<CriticalExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            services.AddScoped<IWeatherService, WeatherService>();

            services.Configure<CustomTokenOptions>(configuration.GetSection("TokenOptions"));
            services.Configure<Clients>(configuration.GetSection("Clients"));
            services.AddScoped<ITokenService,TokenService>();
        }
    }
}
