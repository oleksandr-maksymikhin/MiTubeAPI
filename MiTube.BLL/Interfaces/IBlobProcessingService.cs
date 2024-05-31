using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.BLL.Interfaces
{
    public interface IBlobProcessingService
    {
        public Task<string> UploadFile(IFormFile fileToUpload);
        public Task RemoveFile(string url);
        public Task<Stream> DownloadFile(string url);

    }
}
