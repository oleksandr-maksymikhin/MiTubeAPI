using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        //public String? PosterType { get; set; }
        public IFormFile? BanerFile { get; set; }
        //public String? BanerType { get; set; }
    }
}
