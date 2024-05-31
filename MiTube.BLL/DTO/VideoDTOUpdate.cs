using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.BLL.DTO
{
    public class VideoDTOUpdate
    {
        public Guid Id { get; set; }
        //public Guid UserId { get; set; }            //??????? userId or User
        public String? Title { get; set; }
        //public IFormFile VideoFile { get; set; }
        public IFormFile? PosterFile { get; set; }
        public String? Description { get; set; }
        public bool IsPublic { get; set; }
        //public DateTime? Date { get; set; }
        //public int Duration { get; set; }
    }
}
