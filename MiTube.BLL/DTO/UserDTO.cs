using Microsoft.AspNetCore.Mvc;
using MiTube.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.BLL.DTO
{
    public class UserDTO
    {
        //get from User entity
        public Guid Id { get; set; }
        public String UserTypeDescription { get; set; }
        public String? Name { get; set; }
        public String? Nickname { get; set; }
        public String? Description { get; set; }
        public String PosterUrl { get; set; }
        //public String? PosterType { get; set; }
        public String BanerUrl { get; set; }
        //public String? BanerType { get; set; }

        //get from other entities
        public String Email { get; set; }
        public bool IsPremium { get; set; } = false;
        public int SubscribersQuantity { get; set; }
        public int VideosQuantity { get; set; }
        public int CommentsQuantity { get; set; }
        public int VideoViewQuantity { get; set; }
        public int AllVideosDuration { get; set; }
        public Guid? WatchLaterPlaylistId { get; set; }


        //public ICollection<Comment>? Comments { get; set; }
        //public ICollection<User>? Subscribers { get; set; }
        //public ICollection<Subscription>? Subscriptions { get; set; }
        //public ICollection<Video>? Videos { get; set; }
        //public ICollection<Playlist>? Playlists { get; set; }
    }
}
