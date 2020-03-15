using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using PholdApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PholdApi.Services
{
    public class PholdStorageService : IPholdStorageService
    {
        private readonly IConfiguration _config;
        private readonly string _connString;

        private BlobContinuationToken continuationToken = null;

        public PholdStorageService(IConfiguration config)
        {
            _config = config;
            _connString = _config.GetSection("StorageAccountConnectionString").Value;
        }

        public async Task<List<Uri>> GetImagesAsync(int id)
        {
            CloudStorageAccount storageAccount;
            if (!CloudStorageAccount.TryParse(_connString, out storageAccount))
            {
                throw new Exception("Unable to connect to storage");
            }

            var fileClient = storageAccount.CreateCloudBlobClient();
            var share = fileClient.GetContainerReference("phold");

            var sharedPolicy = new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(1),
                Permissions = SharedAccessBlobPermissions.Read
            };

            var sasToken = share.GetSharedAccessSignature(sharedPolicy);

            var rootDir = share.GetDirectoryReference("photos/" + id);

            var resultSegment = await rootDir.ListBlobsSegmentedAsync(continuationToken);

            var idList = new List<Uri>();
            foreach (var item in resultSegment.Results)
            {
                var fileSasUri = new Uri(item.StorageUri.PrimaryUri.ToString() + sasToken);

                idList.Add(fileSasUri);

            }

            return idList;
        }
    }
}
