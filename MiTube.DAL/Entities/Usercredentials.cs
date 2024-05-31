using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.DAL.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class Usercredentials : Model
    {
        [Required]
        [ForeignKey("FK_User_Id")]
        public Guid UserId { get; set; }
        [Required]
        public String Email { get; set; }
        [Required]
        public String Password { get; set; }

    }


}
