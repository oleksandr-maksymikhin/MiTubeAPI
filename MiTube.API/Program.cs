using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiTube.BLL.Infrastructure;
using MiTube.BLL.Interfaces;
using MiTube.BLL.Services;
using MiTube.DAL.Entities;
using MiTube.DAL.Interfaces;
using MiTube.DAL.Repositories;




//!!!!!!!!!!!!!!!!!!!!  set a mechanism to transfer connection string from API to DAL  !!!!!!!!!!!!!!!

namespace MiTube.API
{
    public class Program
    {
        public static void Main(string[] args)
       {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<UserService, UserService>();
            builder.Services.AddScoped<IUsercredentialsService, UsercredentialsService>();
            builder.Services.AddScoped<UsercredentialsService, UsercredentialsService>();
            builder.Services.AddScoped<IVideoService, VideoService>();
            builder.Services.AddScoped<IShowVideoService, ShowVideoService>();
            builder.Services.AddScoped<IShowImageService, ShowImageService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<IInteractionService, InteractionService>();
            builder.Services.AddScoped<InteractionService, InteractionService>();
            builder.Services.AddScoped<IPlaylistService, PlaylistService>();
            builder.Services.AddScoped<PlaylistService, PlaylistService>();
            builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
            builder.Services.AddScoped<SubscriptionService, SubscriptionService>();

            //BindingAddress automapper
            //builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            ////connection string to MSSQL will be passes to DAL level via extension methods to builder.Services
            //builder.Services.AddDbContext<MiTubeAPIContext>(options =>
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("MiTubeAPIContext") ?? throw new InvalidOperationException("Connection string 'MiTubeAPIContext' not found.")));



            //string dbConnectionString = builder.Configuration.GetConnectionString("MiTubeAPIContext");
            string dbConnectionString = builder.Configuration.GetConnectionString("MiTubeAPIContextAzure");
            builder.Services.SetBllDependencies(dbConnectionString);


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();




            //add connectin to Azure Blob
            //builder.Services.AddSingleton(provider => {
            //    IConfigurationRoot configuration = new ConfigurationBuilder().
            //    AddJsonFile("appsettings.json").
            //    Build();
            //    return configuration;
            //});

            IConfigurationRoot configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.json").
                Build();

            string connectionString = configuration["StorageConnectionString"];
            string blobContainerName = configuration["BlobContainerNameString"];

            ////that works
            //builder.Services.AddSingleton(provider => {
            //    BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            //    return blobServiceClient;
            //});

            ////that works
            //builder.Services.AddScoped(provider => {
            //    BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            //    return blobServiceClient;
            //});

            builder.Services.AddScoped(provider => {
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
                bool isExist = blobContainerClient.Exists();
                if (!isExist)
                {
                    blobContainerClient = blobServiceClient.CreateBlobContainer(blobContainerName);
                }

                return blobContainerClient;
            });

            //add session in services
            //builder.Services.AddSession(options => options.IdleTimeout = TimeSpan.FromSeconds(300));

            builder.Services.AddDistributedMemoryCache();           //for distributed systems
            //builder.Services.AddMemoryCache();                    //for one server non-distributed systems

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                //options.Cookie.Domain = "app-beevi-web-eastus-dev-001.azurewebsites.net";
                //options.Cookie.SameSite = SameSiteMode.None;
                //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;


                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.Domain = "app-beevi-web-eastus-dev-001.azurewebsites.net";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.HttpOnly = true;


                //options.Cookie.Name = "MiTubeSessionCookie";            //name of the cookie
                //options.Cookie.HttpOnly = true;                     //cookie is only available to code executing on the server
                //options.Cookie.IsEssential = true;
                //options.IOTimeout = TimeSpan.FromMinutes(1);
            });

            builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
            {
                builder.WithOrigins("http://192.168.1.100:4200", "http://localhost:4200", "http://localhost:3000", "https://beevi.azurewebsites.net").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                
                //builder.AllowAnyOrigin()
                //   .AllowAnyMethod()
                //   .AllowAnyHeader()
                //   .AllowCredentials();
            }));


            //string dbConnectionString = builder.Configuration.GetConnectionString("MiTubeAPIContext");
            //builder.Services.SetBllDependencies(dbConnectionString);

            var app = builder.Build();

            //use session in middleware
            app.UseSession();
            //Cors
            app.UseCors("corsapp");

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                app.UseSwaggerUI();
            }

            //use session in middleware
            //app.UseSession();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            //use session in middleware
            //app.UseSession();

            app.MapControllers();

                app.Run();
        }
    }
}
