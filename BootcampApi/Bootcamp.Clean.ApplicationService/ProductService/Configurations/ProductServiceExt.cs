//using Bootcamp.Repository.Repositories.ProductRepositories;
//using Bootcamp.Service.ProductService.Helpers;
//using Bootcamp.Service.ProductService.ProductServices;
//using FluentValidation;
//using Microsoft.Extensions.DependencyInjection;

//namespace Bootcamp.Clean.ApplicationService.ProductService.Configurations
//{
//    public static class ProductServiceExt
//    {
//        public static IServiceCollection AddProductService(this IServiceCollection services)
//        {
//            services.AddScoped<ISyncProductService, SyncProductService>();
//            services.AddScoped<ISyncProductRepository, SyncProductRepository>();

//            services.AddScoped<IAsyncProductService, AsyncProductService>();
//            services.AddScoped<IAsyncProductRepository, AsyncProductRepository>();

//            //builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
//            services.AddValidatorsFromAssemblyContaining<ProductCreateRequestValidator>();
//            services.AddScoped<NotFoundFilter>();

//            services.AddSingleton<PriceCalculator>();

//            return services;
//        }
//    }
//}
