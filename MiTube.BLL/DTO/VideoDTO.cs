
namespace MiTube.BLL.DTO
{
    public class VideoDTO
    {
        public Guid Id { get; set; }
        public UserDTO User { get; set; }
        public String Title { get; set; }
        public String VideoUrl { get; set; }
        public String PosterUrl { get; set; }
        public String Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime? Date { get; set; }
        public int Duration { get; set; }

        public int Likecount { get; set; }
        public int Dislikecount { get; set; }
        public int Views { get; set; }
        public int Commentscount { get; set; }
        public float LikeRate { get; set; }
        public int LikeStatus { get; set; }
    }
}
