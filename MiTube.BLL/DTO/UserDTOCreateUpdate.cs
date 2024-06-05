using Microsoft.AspNetCore.Http;

namespace MiTube.BLL.DTO
{
    public class UserDTOCreateUpdate
    {
        public Guid Id { get; set; }
        public int UserTypeId { get; set; }
        public String? Name { get; set; }
        public String? Nickname { get; set; }
        public String? Description { get; set; }
        public IFormFile? PosterFile { get; set; }
        public IFormFile? BanerFile { get; set; }
    }
}
