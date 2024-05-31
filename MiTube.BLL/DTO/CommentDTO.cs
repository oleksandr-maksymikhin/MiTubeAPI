using MiTube.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.BLL.DTO
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public Guid VideoId { get; set; }

        public String Value { get; set; }

        public Guid? ParentId { get; set; }

        public DateTime? Timestamp { get; set; }
    }
}
