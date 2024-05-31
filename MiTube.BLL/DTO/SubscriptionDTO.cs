using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.BLL.DTO
{
    public class SubscriptionDTO
    {
        public Guid Id { get; set; }
        public Guid PublisherId { get; set; }
        public Guid SubscriberId { get; set; }

    }
}
