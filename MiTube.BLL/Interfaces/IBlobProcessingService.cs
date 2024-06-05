using Microsoft.AspNetCore.Http;

namespace MiTube.BLL.Interfaces
{
    public interface IBlobProcessingService
    {
        public Task<string> UploadFile(IFormFile fileToUpload);
        public Task RemoveFile(string url);
        public Task<Stream> DownloadFile(string url);

    }
}
