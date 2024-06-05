using Azure.Storage.Blobs;
using MiTube.BLL.Interfaces;

namespace MiTube.BLL.Services
{
    public class ShowImageService : IShowImageService
    {
        BlobContainerClient blobContainerClient;

        public ShowImageService(BlobContainerClient blobContainerClient)
        {
            this.blobContainerClient = blobContainerClient;
        }

        public async Task<Stream> ShowImageByBlobUrl(string url)
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
