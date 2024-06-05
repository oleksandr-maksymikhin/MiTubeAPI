using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using MiTube.BLL.Interfaces;

namespace MiTube.BLL.Services
{
    public class BlobProcessingService : IBlobProcessingService
    {
        BlobContainerClient blobContainerClient;
        public BlobProcessingService(BlobContainerClient blobContainerClient)
        {
            this.blobContainerClient = blobContainerClient;
        }

        public async Task<string> UploadFile(IFormFile fileToUpload)
        {
            String blobName = fileToUpload.FileName;
            String blobExtention = Path.GetExtension(blobName);
            String modifiedBlobName = blobName.Replace(blobExtention, "_" + DateTime.Now.ToString() + blobExtention);
            BlobClient blobClient = blobContainerClient.GetBlobClient(modifiedBlobName);
            using (var fileStream = fileToUpload.OpenReadStream())
            {
                await blobClient.UploadAsync(fileStream, true);
            }
            string downloadUrl = blobClient.Uri.AbsoluteUri;
            return downloadUrl;
        }

        public async Task RemoveFile(string url)
        {
            BlobClient blobClient = new BlobClient(new Uri(url));
            String blobName = blobClient.Name;
            blobClient = blobContainerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
            return;
        }

        public async Task<Stream> DownloadFile(string url)
        {
            try
            {
                String blobName = new BlobClient(new Uri(url)).Name;
                BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

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
