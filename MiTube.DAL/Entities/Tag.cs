using System;
using System.ComponentModel.DataAnnotations;

namespace MiTube.DAL.Entities
{
    public class Tag : Model
    {
        [Required, StringLength(64)]
        public String Name { get; set; }

        virtual public ICollection<Video> Videos { get; set; }
    }
}