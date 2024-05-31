using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiTube.DAL.Infrastructure;
using MiTube.DAL.Repositories;
using MiTube.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using MiTube.DAL.Context;
using MiTube.BLL.Services;
using MiTube.DAL.Entities;
using AutoMapper;
using MiTube.BLL.DTO;
using Microsoft.AspNetCore.Routing.Constraints;
using MiTube.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MiTube.BLL.Infrastructure
{
    public static class BllDependancyProvider
    {
        public static IServiceCollection SetBllDependencies(this IServiceCollection services, string connectionString)
        {
            services.SetDalDependencies(connectionString);
            //services.AddScoped<GenericRepositoryIdGuid<User>, UserRepository>();
            services.AddScoped<UserRepository, UserRepository>();
            //services.AddScoped<GenericRepositoryIdGuid<Video>, VideoRepository>();
            services.AddScoped<VideoRepository, VideoRepository>();
            services.AddScoped<InteractionRepository, InteractionRepository>();
            //services.AddScoped<GenericRepositoryIdGuid<Usercredentials>, UsercredentialsRepository>();
            services.AddScoped<UsercredentialsRepository, UsercredentialsRepository>();

            services.AddScoped<ICommentRepository, CommentRepository>();
            //services.AddScoped<IRepositoryIdGuid<Video>, VideoRepository>();
            services.AddScoped<IVideoRepository, VideoRepository>();
            services.AddScoped<IInteractionRepository, InteractionRepository>();
            //services.AddScoped<IRepositoryIdGuid<User>, UserRepository>();

            //services.AddScoped<IRepositoryIdGuid<Interaction>, GenericRepositoryIdGuid<Interaction>>();
            services.AddScoped<IPlaylistRepository, PlaylistRepository>();
            services.AddScoped<PlaylistRepository, PlaylistRepository>();
            //services.AddScoped<HttpContext, HttpContext>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<SubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUsercredentialsRepository, UsercredentialsRepository>();

            services.AddScoped<IBlobProcessingService, BlobProcessingService>();
            //services.AddSingleton<IBlobProcessingService, BlobProcessingService>();
            //services.AddSingleton<BlobProcessingService, BlobProcessingService>();                    //work
            services.AddScoped<BlobProcessingService, BlobProcessingService>();                         //work

            //Add mapper
            services.AddScoped(provider =>
                new MapperConfiguration(Configure).CreateMapper()
            );

            ////if use UnitOfWork
            //services.AddScoped<IUnitOfWork, EFUnitOfWork>();
            return services;
        }


        public static void Configure(IMapperConfigurationExpression config)
        {
            //config.CreateMap<UserDTO, User>().ReverseMap();
            //mapperUser = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>().ReverseMap()).CreateMapper();

            //************* UserCredentias ************
            config.CreateMap<Usercredentials, UsercredentialsDTO>().ReverseMap();

            //************* User ************
            config.CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserTypeDescription, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.Nickname))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.PosterUrl, opt => opt.MapFrom(src => src.PosterUrl))
                .ForMember(dest => dest.BanerUrl, opt => opt.MapFrom(src => src.BanerUrl))
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.IsPremium, opt => opt.Ignore());

            config.CreateMap<UserDTO, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserTypeId, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.Nickname))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.PosterUrl, opt => opt.MapFrom(src => src.PosterUrl))
                .ForMember(dest => dest.BanerUrl, opt => opt.MapFrom(src => src.BanerUrl));

            //we dont create UserDTOCreateUpdate from User
            //config.CreateMap<User, UserDTOCreateUpdate>()
            //   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //   .ForMember(dest => dest.UserTypeId, opt => opt.MapFrom(src => src.UserTypeId))
            //   .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            //   .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.Nickname))
            //   .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            //   .ForMember(dest => dest.PosterFile, opt => opt.Ignore())
            //   .ForMember(dest => dest.PosterType, opt => opt.MapFrom(src => src.PosterType))
            //   .ForMember(dest => dest.BanerFile, opt => opt.Ignore())
            //   .ForMember(dest => dest.BanerType, opt => opt.MapFrom(src => src.BanerType));


            config.CreateMap<UserDTOCreateUpdate, User>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.UserTypeId, opt => opt.MapFrom(src => src.UserTypeId))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.Nickname))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dest => dest.PosterUrl, opt => opt.Ignore())
               .ForMember(dest => dest.PosterUrl, opt => opt.Ignore());

            //************* Video ************
            config.CreateMap<VideoDTOCreate, Video>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.VideoUrl, opt => opt.Ignore())
                .ForMember(dest => dest.PosterUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.IsPublic))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration));

            config.CreateMap<Video, VideoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom(src => src.VideoUrl))
                .ForMember(dest => dest.PosterUrl, opt => opt.MapFrom(src => src.PosterUrl))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.IsPublic))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration));

            config.CreateMap<VideoDTO, Video>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
               .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
               .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom(src => src.VideoUrl))
               .ForMember(dest => dest.PosterUrl, opt => opt.MapFrom(src => src.PosterUrl))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.IsPublic))
               .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
               .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration));

            //************* Comment ************
            config.CreateMap<Comment, CommentDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.VideoId, opt => opt.MapFrom(src => src.VideoId))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp));

            config.CreateMap<CommentDTO, Comment>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.VideoId, opt => opt.MapFrom(src => src.VideoId))
                .ForMember(dest => dest.Video, opt => opt.Ignore())
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ForMember(dest => dest.Timestamp, opt => opt.Ignore());

            //************* Playlist ************
            config.CreateMap<PlaylistDTO, Playlist>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.PosterUrl, opt => opt.MapFrom(src => src.PosterUrl))
                .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.IsPublic))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Videos, opt => opt.Ignore());

            config.CreateMap<Playlist, PlaylistDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.PosterUrl, opt => opt.MapFrom(src => src.PosterUrl))
                .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.IsPublic))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));

            config.CreateMap<PlaylistDTOCreate, Playlist>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.PosterUrl, opt => opt.Ignore())
                .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.IsPublic))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Videos, opt => opt.Ignore());


            //config.CreateMap<PlaylistDTO, Playlist>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            //    .ForMember(dest => dest.User, opt => opt.Ignore())
            //    .ForMember(dest => dest.Videos, opt => opt.Ignore())
            //    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            //config.CreateMap<Playlist, PlaylistDTO>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            //    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            //************* Interaction ************
            config.CreateMap<Interaction, InteractionDTO>().ReverseMap();
            //config.CreateMap<Interaction, InteractionDTO>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            //    .ForMember(dest => dest.VideoId, opt => opt.MapFrom(src => src.VideoId))
            //    .ForMember(dest => dest.Actionstate, opt => opt.MapFrom(src => src.Actionstate));

            //config.CreateMap<InteractionDTO, Interaction>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            //    .ForMember(dest => dest.VideoId, opt => opt.MapFrom(src => src.VideoId))
            //    .ForMember(dest => dest.Actionstate, opt => opt.MapFrom(src => src.Actionstate));

            //************* Subscription ************
            config.CreateMap<Subscription, SubscriptionDTO>().ReverseMap();
            
            //************* Tag ************

            

            //************* PremiumUser ************


        }



    }

}
