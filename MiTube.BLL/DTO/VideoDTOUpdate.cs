using Microsoft.AspNetCore.Http;

namespace MiTube.BLL.DTO
{
    public class VideoDTOUpdate
    {
        public Guid Id { get; set; }
        public String? Title { get; set; }
        public IFormFile? PosterFile { get; set; }
        public String? Description { get; set; }
        public bool IsPublic { get; set; }
    }
}
