using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using MiTube.BLL.Infrastructure;
using MiTube.BLL.Interfaces;
using MiTube.BLL.Services;


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

            //string? dbConnectionString = builder.Configuration.GetConnectionString("MiTubeAPIContext");
            string? dbConnectionString = builder.Configuration.GetConnectionString("MiTubeAPIContextAzure");
            if (dbConnectionString != null)
            {
                builder.Services.SetBllDependencies(dbConnectionString);
            }

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            IConfigurationRoot configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.json").
                Build();

            string connectionString = configuration["StorageConnectionString"];
            string blobContainerName = configuration["BlobContainerNameString"];

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

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.Domain = "api-mitube-eastus-dev-001.azurewebsites.net";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.HttpOnly = true;
            });

            builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
            {
                builder.WithOrigins("http://localhost:3000", "https://mitube.azurewebsites.net")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
            }));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSession();
            app.UseCors("corsapp");
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
