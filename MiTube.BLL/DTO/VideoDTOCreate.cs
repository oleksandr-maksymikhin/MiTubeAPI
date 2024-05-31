using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MiTube.DAL.Entities;

namespace MiTube.BLL.DTO
{
    public class VideoDTOCreate
    {
        public Guid UserId { get; set; }            //??????? userId or User
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
