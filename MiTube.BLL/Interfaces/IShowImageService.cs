
namespace MiTube.BLL.Interfaces
{
    public interface IShowImageService
    {
        public Task<Stream> ShowImageByBlobUrl(String url);
    }
}
