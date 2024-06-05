
namespace MiTube.BLL.Interfaces
{
    public interface IShowVideoService
    {
        public Task<Stream> ShowVideoByBlobUrl(String url);
    }
}
