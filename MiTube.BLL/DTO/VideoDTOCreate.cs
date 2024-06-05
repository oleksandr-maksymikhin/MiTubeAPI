using Microsoft.AspNetCore.Http;

namespace MiTube.BLL.DTO
{
    public class VideoDTOCreate
    {
        public Guid UserId { get; set; }
        public String Title { get; set; }
        public IFormFile VideoFile { get; set; }
        public IFormFile PosterFile { get; set; }
        public String Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime? Date { get; set; }
        public int Duration { get; set; }
        public Guid? PlaylistId { get; set; }
    }
}
