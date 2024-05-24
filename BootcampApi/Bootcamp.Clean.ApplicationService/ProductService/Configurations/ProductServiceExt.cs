using Bootcamp.Clean.ApplicationService.ProductService.Helpers;
using Bootcamp.Clean.ApplicationService.ProductService.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Bootcamp.Clean.ApplicationService.ProductService.Configurations
{
    public static class ProductServiceExt
    {
        public static IServiceCollection AddProductService(this IServiceCollection services)
        {
            //    services.AddScoped<IProductRepository, ProductRepository>();

            //    //builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            //    services.AddValidatorsFromAssemblyContaining<ProductCreateRequestValidator>();
            //    services.AddScoped<NotFoundFilter>();
            //    services.AddScoped<ProductService>();

            //    services.AddSingleton<PriceCalculator>();

            return services;
        }
    }
}
