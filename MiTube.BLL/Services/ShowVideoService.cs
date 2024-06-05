using Azure.Storage.Blobs;
using MiTube.BLL.Interfaces;

namespace MiTube.BLL.Services
{
    public class ShowVideoService : IShowVideoService
    {
        BlobContainerClient blobContainerClient;

        public ShowVideoService(BlobContainerClient blobContainerClient)
        {
            this.blobContainerClient = blobContainerClient;
        }

        public async Task<Stream> ShowVideoByBlobUrl(String url)
        {
            try
            {
                BlobClient blobClient = new BlobClient(new Uri(url));
                String blobName = blobClient.Name;
                blobClient = blobContainerClient.GetBlobClient(blobName);
                Stream stream = await blobClient.OpenReadAsync();
                return stream;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
