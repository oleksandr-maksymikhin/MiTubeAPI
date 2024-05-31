using MiTube.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.BLL.DTO
{
    public class VideoDTO
    {
        public Guid Id { get; set; }
        public UserDTO User { get; set; }            //??????? userId or User
        public String Title { get; set; }
        public String VideoUrl { get; set; }
        public String PosterUrl { get; set; }
        public String Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime? Date { get; set; }
        public int Duration { get; set; }

        //countable properties - calculate from other entities during each request is disrupt normalization and store this information in two entities ?????
        public int Likecount { get; set; }
        public int Dislikecount { get; set; }
        public int Views { get; set; }
        //public Guid PlaylistId { get; set; }      //What is it ? It should be in PlayList entity
        public int Commentscount { get; set; }
        public float LikeRate { get; set; }

        //public List<Guid>? Tags { get; set; }   //??????? List<Guid> or List<Tag>
        public int LikeStatus { get; set; }
    }
}
