using Bootcamp.Repository;
using Bootcamp.Repository.Context;
using Bootcamp.Repository.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Repository
{
    public static class RepositoryAssembly
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<AppDbContext>(option =>
            //    option.UseSqlServer(configuration.GetConnectionString("SqlServer"),
            //    option =>
            //    {
            //        option.MigrationsAssembly(typeof(RepositoryAssembly).Assembly.GetName().Name);
            //    }));

            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}