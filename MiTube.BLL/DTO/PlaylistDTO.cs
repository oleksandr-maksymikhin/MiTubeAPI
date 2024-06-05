
namespace MiTube.BLL.DTO
{
    public class PlaylistDTO
    {
        public Guid Id { get; set; }
         public Guid UserId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String PosterUrl { get; set; }
        public bool IsPublic { get; set; }
        public DateTime Date { get; set; }

        //calculated properties
        public int VideoQuantity { get; set; }

    }
}
