using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiTube.DAL.Entities;
using MiTube.DAL.Repositories;

namespace MiTube.DAL.Context
{
    public class MiTubeAPIContext : DbContext
    {
        public DbSet<Usercredentials> Usercredentials { get; set; } = default!;
        public DbSet<User> User { get; set; } = default!;
        public DbSet<UserType> UserType { get; set; } = default!;
        public DbSet<PremiumUser> PremiumUser { get; set; } = default!;
        public DbSet<Video> Video { get; set; } = default!;
        public DbSet<Tag> Tag { get; set; } = default!;
        public DbSet<Comment> Comment { get; set; } = default!;
        public DbSet<Interaction> Interaction { get; set; } = default!;
        public DbSet<Playlist>? Playlist { get; set; }
        public DbSet<Subscription> Subscription { get; set; } = default!;

        public MiTubeAPIContext(DbContextOptions<MiTubeAPIContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Usercredentials>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Video>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Subscription>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Tag>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Comment>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Interaction>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Playlist>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<PremiumUser>().Property(x => x.Id).HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Video>().Property(x => x.Date).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Comment>().Property(x => x.Timestamp).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Playlist>().Property(x => x.Date).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Interaction>().Property(x => x.Date).HasDefaultValueSql("getdate()");


        //EF to create relations in Database Subscriber - Publisher
        modelBuilder.Entity<Subscription>(entity =>
            {
                entity.ToTable("Subscription");

                entity.HasOne(s => s.Subscriber)
                    .WithMany(u => u.Subscriptions)
                    .HasForeignKey(s => s.SubscriberId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.Publisher)
                    .WithMany()
                    .HasForeignKey(s => s.PublisherId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //seed initial data
            Guid adminGuid = Guid.NewGuid();
            Guid homerGuid = Guid.NewGuid();
            Guid bardGuid = Guid.NewGuid();
            Guid test1UserGuid = Guid.NewGuid();

            String userPosterDefault = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/bee_userPosterDefault.jpg";
            String userBannerDefault = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/bee_userBannerDefault.jpg";

            Guid terminator_1_Guid = Guid.NewGuid();
            Guid terminator_2_Guid = Guid.NewGuid();
            Guid terminator_3_Guid = Guid.NewGuid();

            Guid troy_1_Guid = Guid.NewGuid();
            Guid troy_2_Guid = Guid.NewGuid();

            Guid avatar_1_Guid = Guid.NewGuid();
            Guid avatar_2_Guid = Guid.NewGuid();

            //test
            Guid matrix_1_Guid = Guid.NewGuid();
            Guid matrix_2_Guid = Guid.NewGuid();

            Guid interactionTerminator_2_Guid_1 = Guid.NewGuid();
            Guid interactionTerminator_2_Guid_2 = Guid.NewGuid();
            Guid interactionTerminator_1_Guid_1 = Guid.NewGuid();
            Guid interactionTerminator_1_Guid_2 = Guid.NewGuid();
            Guid interactionTest1Matrix_1_Guid = Guid.NewGuid();
            Guid interactionTest1Terminator_1_Guid = Guid.NewGuid();

            Guid commentTerminator_2_Guid_1 = Guid.NewGuid();
            Guid commentTerminator_2_Guid_2 = Guid.NewGuid();
            Guid commentTerminator_2_Guid_3 = Guid.NewGuid();
            Guid commentTest1Matrix_1_Guid = Guid.NewGuid();
            Guid commentTest1Terminator_1_Guid = Guid.NewGuid();

            Guid adminDefaultPublicPlaylistGuid = Guid.NewGuid();
            Guid adminDefaultPrivatePlaylistGuid = Guid.NewGuid();
            Guid adminDefaultPrivateWatchLaterPlaylistGuid = Guid.NewGuid();
            Guid homerDefaultPublicPlaylistGuid = Guid.NewGuid();
            Guid homerDefaultPrivatePlaylistGuid = Guid.NewGuid();
            Guid homerDefaultPrivateWatchLaterPlaylistGuid = Guid.NewGuid();
            Guid bardDefaultPublicPlaylistGuid = Guid.NewGuid();
            Guid bardDefaultPrivatePlaylistGuid = Guid.NewGuid();
            Guid bardDefaultPrivateWatchLaterPlaylistGuid = Guid.NewGuid();
            Guid test1DefaultPublicPlaylistGuid = Guid.NewGuid();
            Guid test1DefaultPrivatePlaylistGuid = Guid.NewGuid();
            Guid test1DefaultPrivateWatchLaterPlaylistGuid = Guid.NewGuid();

            String defaultPublicPlayistName = "public playlist";
            String defaultPrivatePlayistName = "private playlist";
            String defaultPrivateWatchLaterPlayistName = "watch later";
            String defaultPublicPlayistDescription = "this is my public playlist";
            String defaultPrivatePlayistDescription = "this is my private playlist";
            String defaultPrivateWatchLaterPlayistDescription = "never rush";
            String playlistPosterPublicDefault = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/bee_playlistPosterPublicDefault.jpg";
            String playlistPosterPrivateDefault = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/bee_playlistPosterPrivateDefault.jpg";
            String playlistPosterPrivateWatchLaterDefault = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/bee_playlistPosterPrivateWatchLaterDefault.webp";

            Guid suscription_1 = Guid.NewGuid();
            Guid suscription_2 = Guid.NewGuid();
            Guid suscription_3 = Guid.NewGuid();
            Guid suscription_4 = Guid.NewGuid();
            Guid suscription_Test1ToBart = Guid.NewGuid();
            Guid suscription_BartToTest1 = Guid.NewGuid();

            //UserType
            modelBuilder.Entity<UserType>().HasData(
               new UserType()
               {
                   Id = 1,
                   Description = "Administrator",
               },
               new UserType()
               {
                   Id = 2,
                   Description = "Registered user",
               }
            );

            //User
            modelBuilder.Entity<User>().HasData(
               new User()
               {
                   Id = adminGuid,
                   UserTypeId = 1,
                   Name = "admin",
                   Nickname = "admin_Man",
                   Description = "Just sleep",
                   PosterUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/bee_adminPosterDefault.jpg",
                   BanerUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/bee_adminBannerDefault.jpg",
               },
                new User()
               {
                   Id = homerGuid,
                   UserTypeId = 2,
                   Name = "homer",
                   Nickname = "homer_man",
                   Description = "Homer plays poker",
                   PosterUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/Simpson_Homer_Poster.jpeg",
                   BanerUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/Simpson_Homer_Banner.jpg",
               },
                new User()
               {
                   Id = bardGuid,
                   UserTypeId = 2,
                   Name = "bart",
                   Nickname = "bart_man",
                   Description = "Bart plays soliter",
                   PosterUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/Simpson_Bart_Poster.jpg",
                   BanerUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/Simpson_Bart_Banner.jpg",
                },
                new User()
                {
                    Id = test1UserGuid,
                    UserTypeId = 2,
                    Name = "QA1",
                    Nickname = "QA1_tester",
                    Description = "Test everything",
                    PosterUrl = userPosterDefault,
                    BanerUrl = userBannerDefault,
                }
            );

            //Usercredentials
            modelBuilder.Entity<Usercredentials>().HasData(
               new Usercredentials()
               {
                   Id = Guid.NewGuid(),
                   UserId = adminGuid,
                   Email = "maks.alex@gmail.com",
                   Password = "admin"
               },
                new Usercredentials()
               {
                   Id = Guid.NewGuid(),
                   UserId = homerGuid,
                   Email = "maks.alex1@gmail.com",
                   Password = "homer"
               },
                new Usercredentials()
                {
                    Id = Guid.NewGuid(),
                    UserId = bardGuid,
                    Email = "maks.alex3@gmail.com",
                    Password = "bart"
                },
                new Usercredentials()
                {
                    Id = Guid.NewGuid(),
                    UserId = test1UserGuid,
                    Email = "testEmail1",
                    Password = "test1"
                }
            );

            //Video
            modelBuilder.Entity<Video>().HasData(
              new Video()
              {
                  Id = terminator_1_Guid,
                  UserId = bardGuid,
                  Title = "Terminator_1",
                  VideoUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/The%20Terminator_short_1.mp4",
                  PosterUrl= "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/terminator_1.jpg",
                  Description = "Terminator_1 trailer",
                  IsPublic = true,
                  Duration = 11
              },
              new Video()
              {
                  Id = terminator_2_Guid,
                  UserId = bardGuid,
                  Title = "Terminator_2",
                  VideoUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/The%20Terminator_short_2.mp4",
                  PosterUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/terminator_2.jpg",
                  Description = "Terminator_2 trailer",
                  IsPublic = true,
                  Duration = 12
              },
              new Video()
              {
                  Id = terminator_3_Guid,
                  UserId = bardGuid,
                  Title = "Terminator_3",
                  VideoUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/The%20Terminator_short_3.mp4",
                  PosterUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/terminator_3.jpg",
                  Description = "Terminator_3 trailer",
                  IsPublic = true,
                  Duration = 10
              },

              new Video()
              {
                  Id = troy_1_Guid,
                  UserId = bardGuid,
                  Title = "Troy_1",
                  VideoUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/Troy_short_1.mp4",
                  PosterUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/troy_1.jpg",
                  Description = "Troy_1 trailer",
                  IsPublic = false,
                  Duration = 20
              },
              new Video()
              {
                  Id = troy_2_Guid,
                  UserId = bardGuid,
                  Title = "Troy_2",
                  VideoUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/Troy_short_2.mp4",
                  PosterUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/troy_2.jpg",
                  Description = "Troy_2 trailer",
                  IsPublic = false,
                  Duration = 20
              },

              new Video()
              {
                  Id = avatar_1_Guid,
                  UserId = homerGuid,
                  Title = "Avatar_1",
                  VideoUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/avatar_short_1.mp4",
                  PosterUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/avatar_1.jpg",
                  Description = "Avatar_1 trailer",
                  IsPublic = true,
                  Duration = 23
              },
              new Video()
              {
                  Id = avatar_2_Guid,
                  UserId = homerGuid,
                  Title = "Avatar_2",
                  VideoUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/avatar_short_2.mp4",
                  PosterUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/avatar_2.jpg",
                  Description = "Avatar_2 trailer",
                  IsPublic = true,
                  Duration = 15
              },

              new Video()
              {
                  Id = matrix_1_Guid,
                  UserId = test1UserGuid,
                  Title = "Matrix_1",
                  VideoUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/matrix_short_1.mp4",
                  PosterUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/matrix_1.jpg",
                  Description = "Matrix_1 trailer",
                  IsPublic = true,
                  Duration = 14
              },
              new Video()
              {
                  Id = matrix_2_Guid,
                  UserId = test1UserGuid,
                  Title = "Matrix_2",
                  VideoUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/matrix_short_2.mp4",
                  PosterUrl = "https://oleksandrmaksymikhin.blob.core.windows.net/mitube/matrix_2.jpg",
                  Description = "Matrix_2 trailer",
                  IsPublic = false,
                  Duration = 14
              }
            );

            //interaction
            modelBuilder.Entity<Interaction>().HasData(
              new Interaction()
              {
                  Id = interactionTerminator_2_Guid_1,
                  UserId = homerGuid,
                  VideoId = terminator_2_Guid,
                  Actionstate = 1
              },
              new Interaction()
              {
                  Id = interactionTerminator_2_Guid_2,
                  UserId = bardGuid,
                  VideoId = terminator_2_Guid,
                  Actionstate = 2
              },
              new Interaction()
              {
                  Id = interactionTerminator_1_Guid_1,
                  UserId = homerGuid,
                  VideoId = terminator_1_Guid,
                  Actionstate = 3
              },
              new Interaction()
              {
                  Id = interactionTerminator_1_Guid_2,
                  UserId = bardGuid,
                  VideoId = terminator_1_Guid,
                  Actionstate = 3
              },
              new Interaction()
              {
                  Id = interactionTest1Matrix_1_Guid,
                  UserId = bardGuid,
                  VideoId = matrix_1_Guid,
                  Actionstate = 2
              },
              new Interaction()
              {
                  Id = interactionTest1Terminator_1_Guid,
                  UserId = test1UserGuid,
                  VideoId = terminator_1_Guid,
                  Actionstate = 3
              }
            );

            //comment
            modelBuilder.Entity<Comment>().HasData(
              new Comment()
              {
                  Id = commentTerminator_2_Guid_1,
                  UserId = homerGuid,
                  VideoId = terminator_2_Guid,
                  Value = "cool film"
              },
              new Comment()
              {
                  Id = commentTerminator_2_Guid_2,
                  UserId = bardGuid,
                  VideoId = terminator_2_Guid,
                  ParentId = commentTerminator_2_Guid_1,
                  Value = "I agree with you"
              },
              new Comment()
              {
                  Id = commentTerminator_2_Guid_3,
                  UserId = homerGuid,
                  VideoId = terminator_2_Guid,
                  Value = "I have changed my opinion"
              },
              new Comment()
              {
                  Id = commentTest1Matrix_1_Guid,
                  UserId = bardGuid,
                  VideoId = matrix_1_Guid,
                  Value = "Bard likes Matrix"
              },
              new Comment()
              {
                  Id = commentTest1Terminator_1_Guid,
                  UserId = test1UserGuid,
                  VideoId = terminator_1_Guid,
                  Value = "Test user likes Terminator"
              }
            );
            //Playlist
            modelBuilder.Entity<Playlist>().HasData(
                new Playlist() 
                { 
                    Id = adminDefaultPublicPlaylistGuid, 
                    UserId = adminGuid, 
                    Name = defaultPublicPlayistName,
                    Description = defaultPublicPlayistDescription,
                    PosterUrl = playlistPosterPublicDefault,
                    IsPublic = true,
                },
                new Playlist()
                {
                    Id = adminDefaultPrivatePlaylistGuid,
                    UserId = adminGuid,
                    Name = defaultPrivatePlayistName,
                    Description = defaultPrivatePlayistDescription,
                    PosterUrl = playlistPosterPrivateDefault,
                    IsPublic = false,
                },
                new Playlist()
                {
                    Id = adminDefaultPrivateWatchLaterPlaylistGuid,
                    UserId = adminGuid,
                    Name = defaultPrivateWatchLaterPlayistName,
                    Description = defaultPrivateWatchLaterPlayistDescription,
                    PosterUrl = playlistPosterPrivateWatchLaterDefault,
                    IsPublic = false,
                },

                new Playlist()
                {
                    Id = homerDefaultPublicPlaylistGuid,
                    UserId = homerGuid,
                    Name = defaultPublicPlayistName,
                    Description = defaultPublicPlayistDescription,
                    PosterUrl = playlistPosterPublicDefault,
                    IsPublic = true,
                },
                new Playlist()
                {
                    Id = homerDefaultPrivatePlaylistGuid,
                    UserId = homerGuid,
                    Name = defaultPrivatePlayistName,
                    Description = defaultPrivatePlayistDescription,
                    PosterUrl = playlistPosterPrivateDefault,
                    IsPublic = false,
                },
                new Playlist()
                {
                    Id = homerDefaultPrivateWatchLaterPlaylistGuid,
                    UserId = homerGuid,
                    Name = defaultPrivateWatchLaterPlayistName,
                    Description = defaultPrivateWatchLaterPlayistDescription,
                    PosterUrl = playlistPosterPrivateWatchLaterDefault,
                    IsPublic = false,
                },

                new Playlist()
                {
                    Id = bardDefaultPublicPlaylistGuid,
                    UserId = bardGuid,
                    Name = defaultPublicPlayistName,
                    Description = defaultPublicPlayistDescription,
                    PosterUrl = playlistPosterPublicDefault,
                    IsPublic = true,
                },
                new Playlist()
                {
                    Id = bardDefaultPrivatePlaylistGuid,
                    UserId = bardGuid,
                    Name = defaultPrivatePlayistName,
                    Description = defaultPrivatePlayistDescription,
                    PosterUrl = playlistPosterPrivateDefault,
                    IsPublic = false,
                },
                new Playlist()
                {
                    Id = bardDefaultPrivateWatchLaterPlaylistGuid,
                    UserId = bardGuid,
                    Name = defaultPrivateWatchLaterPlayistName,
                    Description = defaultPrivateWatchLaterPlayistDescription,
                    PosterUrl = playlistPosterPrivateWatchLaterDefault,
                    IsPublic = false,
                },

                new Playlist()
                    {
                        Id = test1DefaultPublicPlaylistGuid,
                        UserId = test1UserGuid,
                        Name = defaultPublicPlayistName,
                        Description = defaultPublicPlayistDescription,
                        PosterUrl = playlistPosterPublicDefault,
                        IsPublic = true,
                    },
                new Playlist()
                {
                    Id = test1DefaultPrivatePlaylistGuid,
                    UserId = test1UserGuid,
                    Name = defaultPrivatePlayistName,
                    Description = defaultPrivatePlayistDescription,
                    PosterUrl = playlistPosterPrivateDefault,
                    IsPublic = false,
                },
                new Playlist()
                {
                    Id = test1DefaultPrivateWatchLaterPlaylistGuid,
                    UserId = test1UserGuid,
                    Name = defaultPrivateWatchLaterPlayistName,
                    Description = defaultPrivateWatchLaterPlayistDescription,
                    PosterUrl = playlistPosterPrivateWatchLaterDefault,
                    IsPublic = false,
                }
            );

            //PlaylistVideo
            modelBuilder.Entity("PlaylistVideo").HasData(
                new { PlaylistsId = bardDefaultPublicPlaylistGuid, VideosId = terminator_1_Guid },
                new { PlaylistsId = bardDefaultPublicPlaylistGuid, VideosId = terminator_2_Guid },
                new { PlaylistsId = bardDefaultPublicPlaylistGuid, VideosId = terminator_3_Guid },

                new { PlaylistsId = bardDefaultPrivatePlaylistGuid, VideosId = troy_1_Guid },
                new { PlaylistsId = bardDefaultPrivatePlaylistGuid, VideosId = troy_2_Guid },

                new { PlaylistsId = homerDefaultPublicPlaylistGuid, VideosId = avatar_1_Guid },
                new { PlaylistsId = homerDefaultPublicPlaylistGuid, VideosId = avatar_2_Guid },

                new { PlaylistsId = test1DefaultPublicPlaylistGuid, VideosId = matrix_1_Guid },
                new { PlaylistsId = test1DefaultPrivatePlaylistGuid, VideosId = matrix_2_Guid },

                new { PlaylistsId = bardDefaultPrivateWatchLaterPlaylistGuid, VideosId = avatar_1_Guid },
                new { PlaylistsId = bardDefaultPrivateWatchLaterPlaylistGuid, VideosId = avatar_2_Guid },
                new { PlaylistsId = bardDefaultPrivateWatchLaterPlaylistGuid, VideosId = matrix_1_Guid },
                new { PlaylistsId = homerDefaultPrivateWatchLaterPlaylistGuid, VideosId = terminator_1_Guid },
                new { PlaylistsId = homerDefaultPrivateWatchLaterPlaylistGuid, VideosId = terminator_2_Guid },
                new { PlaylistsId = homerDefaultPrivateWatchLaterPlaylistGuid, VideosId = terminator_3_Guid }
            );
            //Subscription
            modelBuilder.Entity<Subscription>().HasData(
                new { Id = suscription_1, PublisherId = bardGuid, SubscriberId = adminGuid },
                new { Id = suscription_2, PublisherId = bardGuid, SubscriberId = homerGuid },
                new { Id = suscription_3, PublisherId = homerGuid, SubscriberId = bardGuid },
                new { Id = suscription_4, PublisherId = adminGuid, SubscriberId = bardGuid },
                new { Id = suscription_Test1ToBart, PublisherId = bardGuid, SubscriberId = test1UserGuid },
                new { Id = suscription_BartToTest1, PublisherId = test1UserGuid, SubscriberId = bardGuid }
            );
        }
    }
}


