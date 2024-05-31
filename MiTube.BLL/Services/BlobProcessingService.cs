using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using MiTube.BLL.Interfaces;
using MiTube.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiTube.BLL.Services
{
    public class BlobProcessingService : IBlobProcessingService
    {
        //BlobServiceClient blobServiceClient;
        //string blobContainerName = "mitube";
        BlobContainerClient blobContainerClient;

        //public BlobProcessingService(BlobServiceClient blobServiceClient)
        public BlobProcessingService(BlobContainerClient blobContainerClient)
        {
            this.blobContainerClient = blobContainerClient;
            //this.blobServiceClient = blobServiceClient;

            //blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
            //bool isExist = blobContainerClient.Exists();
            //if (!isExist)
            //{
            //    blobContainerClient = blobServiceClient.CreateBlobContainer(blobContainerName);
            //}
        }

        //private BlobContainerClient GetBlobContainerClient()
        //{
        //    BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
        //    bool isExist = blobContainerClient.Exists();
        //    if (!isExist)
        //    {
        //        blobContainerClient = blobServiceClient.CreateBlobContainer(blobContainerName);
        //    }
        //    return blobContainerClient;
        //}

        public async Task<string> UploadFile(IFormFile fileToUpload)
        {
            //BlobContainerClient blobContainerClient = GetBlobContainerClient();

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
            //BlobContainerClient blobContainerClient = GetBlobContainerClient();
            
            BlobClient blobClient = new BlobClient(new Uri(url));
            String blobName = blobClient.Name;
            blobClient = blobContainerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
            return;
        }

        public async Task<Stream> DownloadFile(string url)
        {
            //BlobContainerClient blobContainerClient = GetBlobContainerClient();
            
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
