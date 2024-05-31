using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiTube.DAL.Entities
{
    public class Interaction : Model
    {
        [Required]
        [ForeignKey("FK_User_Id")]
        public Guid UserId { get; set; }
        
        [Required]
        [ForeignKey("FK_Video_Id")]
        public Guid VideoId { get; set; }

        [Required]
        public int Actionstate { get; set; }
        public DateTime? Date { get; set; }

        //public DateTime? Date1 { get; set; } = default(DateTime?);

        //properties navigation
        //virtual public User Users { get; set; }
        //virtual public Video Videos { get; set; }
    }
}