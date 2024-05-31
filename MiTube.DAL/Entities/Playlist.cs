using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiTube.DAL.Entities
{
    public class Playlist : Model
    {
        [Required]
        [ForeignKey("FK_User_Id")]
        public Guid UserId { get; set; }

        [Required, StringLength(128)]
        public String Name { get; set; }

        [StringLength(1024)]
        public String Description { get; set; }

        public String PosterUrl { get; set; }
        public bool IsPublic { get; set; }
        public DateTime? Date { get; set; }

        //properties navigation
        //[DeleteBehavior(DeleteBehavior.Cascade)]
        virtual public User User { get; set; }
        virtual public ICollection<Video> Videos { get; set; }
    }
}