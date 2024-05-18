using Bootcamp.Repository;
using Bootcamp.Repository.Context;
using Bootcamp.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bootcamp.Api.Extensions
{
    public static class RepositoryExt
    {
        public static void AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(x =>
            {
                x.UseSqlServer(configuration.GetConnectionString("SqlServer"),
                    x => { x.MigrationsAssembly(typeof(RepositoryAssembly).Assembly.GetName().Name); });
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
