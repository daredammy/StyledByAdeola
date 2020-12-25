using Azure;
using Azure.Storage.Blobs;
using Azure.Storage;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using StyledByAdeola.Models;
using StyledByAdeola.ServiceContracts;

namespace StyledByAdeola.Services
{
    public class BlobStorageService: IBlobStorage
    {
        // Create a BlobServiceClient object which will be used to create a container client
        private BlobServiceClient blobServiceClient; 

        public BlobStorageService(Uri serviceUrl, string acountName, string accountKey)
        {
            // Create StorageSharedKeyCredentials object by reading
            // the values from the configuration (appsettings.json)
            StorageSharedKeyCredential storageCredentials =
                new StorageSharedKeyCredential(acountName, accountKey);

            blobServiceClient = new BlobServiceClient(serviceUrl, storageCredentials);

            foreach (BlobContainers blobContainer in Enum.GetValues(typeof(BlobContainers)))
            {
                try
                {
                    blobServiceClient.CreateBlobContainerAsync(blobContainer.ToString());
                }
                catch (Exception)
                {

                }
            }
        }

        public async Task UploadFileToStorage(BlobContainers containerName, string fileName, Stream fileStream)
        {
            BlobContainerClient containerClient =  blobServiceClient.GetBlobContainerClient(containerName.ToString());
            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            Response<BlobContentInfo> BlobUploadResponse = await blobClient.UploadAsync(fileStream, overwrite: true).ConfigureAwait(false);
        }
    }
}
