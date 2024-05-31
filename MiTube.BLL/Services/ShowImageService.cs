using Azure.Storage.Blobs;
using MiTube.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.BLL.Services
{
    public class ShowImageService : IShowImageService
    {
        //BlobServiceClient serviceClient;
        //string blobContainerName = "mitube";

        BlobContainerClient blobContainerClient;

        //public ShowImageService(BlobServiceClient serviceClient)
        public ShowImageService(BlobContainerClient blobContainerClient)
        {
            //this.serviceClient = serviceClient;
            this.blobContainerClient = blobContainerClient;
        }

        public async Task<Stream> ShowImageByBlobUrl(string url)
        {
            try
            {
                //BlobContainerClient blobContainerClient = serviceClient.GetBlobContainerClient(blobContainerName);
                //blobContainerClient = serviceClient.GetBlobContainerClient(blobContainerName);
                //bool isExist = await blobContainerClient.ExistsAsync();
                //if (!isExist)
                //{
                //    blobContainerClient = await serviceClient.CreateBlobContainerAsync(blobContainerName);
                //}


                //Video video = await dbContextClass.Video.Where(x => x.ID == Id).FirstOrDefaultAsync();
                //BlobContainerClient blobContainerClient = serviceClient.GetBlobContainerClient(blobContainerName);
                BlobClient blobClient = new BlobClient(new Uri(url));
                String blobName = blobClient.Name;
                blobClient = blobContainerClient.GetBlobClient(blobName);
                //using (Stream stream = await blobClient.OpenReadAsync())
                //{

                //}
                Stream stream = await blobClient.OpenReadAsync();
                return stream;


                //var content = new System.IO.MemoryStream(file.Result.FileData);
                //var path = Path.Combine(
                //   Directory.GetCurrentDirectory(), "FileDownloaded",
                //   file.Result.FileName);

                //await CopyStream(content, path);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
