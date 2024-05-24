using Bootcamp.Clean.ApplicationService;
using Bootcamp.Clean.Repository;
using Bootcamp.Clean.Repository.Context;
using Bootcamp.Clean.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bootcamp.Clean.Api.Extensions
{
    public static class RepositoryExt
    {
        public static void AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServerCleanDb"),
                    options => { options.MigrationsAssembly("Bootcamp.Clean.Repository"); });
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
