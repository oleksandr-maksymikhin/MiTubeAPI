using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiTube.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

            ////to start migration of DB after start of app
            ////may be I have to pass host as a parameter from API
            //var host = CreateHostBuilder(args).Build();
            //var service = (IServiceScopeFactory)host.Services.GetService(typeof(IServiceScopeFactory));
            //using (var db = service.CreateScope().ServiceProvider.GetService<MiTubeAPIContext>())
            //{
            //    db.Database.Migrate();
            //}
            

            return services;
        }
    }
}

//Your target project 'MiTube.API' doesn't match your migrations assembly 'MiTube.DAL'.
//Either change your target project or change your migrations assembly.

//Change your migrations assembly by using DbContextOptionsBuilder. E.g.
//options.UseSqlServer(connection, b => b.MigrationsAssembly("MiTube.API")).
//By default, the migrations assembly is the assembly containing the DbContext.

//Change your target project to the migrations project by using the Package Manager Console's Default project drop-down list,
//or by executing "dotnet ef" from the directory containing the migrations project.