using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiTube.DAL.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class Usercredentials : Model
    {
        [Required]
        [ForeignKey("FK_User_Id")]
        public Guid UserId { get; set; }
        [Required]
        [StringLength(64)]
        public String Email { get; set; }
        [Required]
        [StringLength(128)]
        public String Password { get; set; }
    }
}
