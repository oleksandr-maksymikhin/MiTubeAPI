using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.BLL.Interfaces
{
    public interface IShowVideoService
    {
        public Task<Stream> ShowVideoByBlobUrl(String url);
    }
}
