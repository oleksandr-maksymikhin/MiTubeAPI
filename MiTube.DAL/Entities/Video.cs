using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiTube.DAL.Entities
{
    [Index(nameof(VideoUrl), IsUnique = true)]
    [Index(nameof(PosterUrl), IsUnique = true)]
    public class Video : Model
    {
        [Required]
        [ForeignKey("FK_User_Id")]
        public Guid UserId { get; set; }
        [Required, StringLength(64)]
        public String Title { get; set; }
        [Required]
        public String VideoUrl { get; set; }
        [Required]
        public String PosterUrl { get; set; }
        [StringLength(1024)]
        public String Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime? Date { get; set; }
        public int Duration { get; set; }


        //properties navigation
        virtual public ICollection<Comment>? Comments { get; set; }
        virtual public ICollection<Interaction>? Interactions { get; set; }
        virtual public ICollection<Playlist>? Playlists { get; set; }
        virtual public ICollection<Tag>? Tags { get; set; }

    }
}