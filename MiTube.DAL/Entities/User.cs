using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiTube.DAL.Entities
{
    
    [Index(nameof(Name), IsUnique = true)]
    public class User : Model
    {
        [Required]
        [ForeignKey("FK_UserType_Id")]
        public int UserTypeId { get; set; }
        public String Name { get; set; }
        public String Nickname { get; set; }
        public String Description { get; set; }
        public String PosterUrl { get; set; }
        public String BanerUrl { get; set; }

        //properties navigation
        public virtual UserType UserType { get; set; }
        public virtual Usercredentials Usercredentials { get; set; }
        public virtual PremiumUser? PremiumUser { get; set; }
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public virtual ICollection<Comment>? Comments { get; set; }
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public virtual ICollection<Interaction>? Interactions { get; set; }
        public virtual ICollection<Subscription>? Subscriptions { get; set; }
        public virtual ICollection<Video>? Videos { get; set; }
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public virtual ICollection<Playlist>? Playlists { get; set; }
    }
}