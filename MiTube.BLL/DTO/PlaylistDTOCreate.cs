using Microsoft.AspNetCore.Http;

namespace MiTube.BLL.DTO
{
    public class PlaylistDTOCreate
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public String Name { get; set; }
        public String? Description { get; set; }
        public IFormFile? PosterFile { get; set; }
        public bool IsPublic { get; set; }
        public DateTime? Date { get; set; }
    }
}
