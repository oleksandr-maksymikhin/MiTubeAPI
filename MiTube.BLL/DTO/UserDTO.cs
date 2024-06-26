﻿
namespace MiTube.BLL.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public String UserTypeDescription { get; set; }
        public String? Name { get; set; }
        public String? Nickname { get; set; }
        public String? Description { get; set; }
        public String PosterUrl { get; set; }
        public String BanerUrl { get; set; }

        public String Email { get; set; }
        public bool IsPremium { get; set; } = false;
        public int SubscribersQuantity { get; set; }
        public int VideosQuantity { get; set; }
        public int CommentsQuantity { get; set; }
        public int VideoViewQuantity { get; set; }
        public int AllVideosDuration { get; set; }
        public Guid? WatchLaterPlaylistId { get; set; }
    }
}
