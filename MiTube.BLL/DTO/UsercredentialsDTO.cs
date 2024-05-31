using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.BLL.DTO
{
    public class UsercredentialsDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
    }
}
