using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiTube.DAL.Context;

namespace MiTube.DAL.Infrastructure
{
    public static class DalDependancyProvider
    {
        public static IServiceCollection SetDalDependencies(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MiTubeAPIContext>(options =>
            {
                options.UseSqlServer(
                    connectionString,
                    b => b.MigrationsAssembly("MiTube.API"));
            });
            services.AddScoped<DbContext, MiTubeAPIContext>();

            return services;
        }
    }
}
