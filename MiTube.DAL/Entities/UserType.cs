using System;
using System.ComponentModel.DataAnnotations;

namespace MiTube.DAL.Entities
{
    public class UserType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public String Description { get; set; }
    }
}