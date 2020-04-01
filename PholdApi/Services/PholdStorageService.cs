using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using PholdApi.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PholdApi.Services
{
    public class PholdStorageService : IPholdStorageService
    {
        private readonly string _connString;

        private BlobContinuationToken continuationToken = null;
        private CloudBlobContainer _container = null;

        public PholdStorageService(IConfiguration config)
        {
            _connString = config.GetSection("StorageAccountConnectionString").Value;
            _container = GetBlobContainer();
        }

        public async Task<List<Uri>> GetImagesAsync(int id)
        {
            var sharedPolicy = new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(1),
                Permissions = SharedAccessBlobPermissions.Read
            };

            var sasToken = _container.GetSharedAccessSignature(sharedPolicy);
            var resultSegment = await GetResultSegmentOfBlobContainer(id);

            var idList = new List<Uri>();
            foreach (var item in resultSegment.Results)
            {
                var fileSasUri = new Uri(item.StorageUri.PrimaryUri.ToString() + sasToken);

                idList.Add(fileSasUri);

            }

            return idList;
        }

        public async Task<string> UploadPhotoAsync(int id, IFormFile file)
        {
            StringBuilder blobName = new StringBuilder(Path.GetFileName(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"')));
            var rootDir = _container.GetDirectoryReference("photos");
            CloudBlockBlob blockBlob = null;

            string firstBlobName = blobName.ToString();
            var newBlobNameIndex = 0;
            // TO DO REFACTOR ---------------------------------------
            do
            {
                if (newBlobNameIndex > 0)
                {
                    blobName.Replace(blobName.ToString(), firstBlobName);
                    blobName.Insert(firstBlobName.IndexOf('.'), $"_{newBlobNameIndex++}");
                }
                else
                    newBlobNameIndex++;

                blockBlob = rootDir.GetBlockBlobReference($"{id}/{blobName.ToString()}");
            } while (await blockBlob.ExistsAsync());
            // TO DO REFACTOR ---------------------------------------
            blockBlob.Properties.ContentType = file.ContentType;
            

            using (var imageStream = file.OpenReadStream())
            {
                var imageBuffer = FileAsByteArray(file, imageStream);
                await blockBlob.UploadFromByteArrayAsync(imageBuffer, 0, (int)file.Length);
                return blobName.ToString();
            }
        }

        private CloudBlobContainer GetBlobContainer()
        {
            CloudStorageAccount storageAccount = null;
            if (!CloudStorageAccount.TryParse(_connString, out storageAccount))
            {
                throw new Exception("Unable to connect to the storage");
            }

            var fileClient = storageAccount.CreateCloudBlobClient();
            var container = fileClient.GetContainerReference("phold");
            return container;
        }

        private async Task<BlobResultSegment> GetResultSegmentOfBlobContainer(int id)
        {
            var rootDir = _container.GetDirectoryReference("photos/" + id);

            var resultSegment = await rootDir.ListBlobsSegmentedAsync(continuationToken);
            return resultSegment;
        }

        private static byte[] FileAsByteArray(IFormFile file, Stream imageStream)
        {
            var contents = new byte[file.Length];

            for (int i = 0; i < file.Length; i++)
            {
                contents[i] = (byte)imageStream.ReadByte();
            }

            return contents;
        }
    }
}
