using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.BLL.Interfaces
{
    public interface IShowImageService
    {
        public Task<Stream> ShowImageByBlobUrl(String url);
    }
}
