using Microsoft.Extensions.Configuration;
using PholdApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Services
{
    public class PholdStorageService : IPholdStorageService
    {
        IConfiguration _config;
        public PholdStorageService(IConfiguration config)
        {
            _config = config;
        }

        public List<Uri> GetImages(int[] ids)
        {

            var _connString = _config.GetSection("StorageAccountConnectionString").Value;
            return new List<Uri>();
            //var _connString = _config.GetSection("StorageAccountConnectionString").Value;
            //CloudStorageAccount storageAccount;
            //if (!CloudStorageAccount.TryParse(_connString, out storageAccount))
            //{
            //    return StatusCode(500, "Unable to connect to storage");
            //}

            //var fileClient = storageAccount.CreateCloudBlobClient();
            //var share = fileClient.GetContainerReference("phold");

            //var sharedPolicy = new SharedAccessBlobPolicy()
            //{
            //    SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(1),
            //    Permissions = SharedAccessBlobPermissions.Read
            //};

            //var sasToken = share.GetSharedAccessSignature(sharedPolicy);

            //var rootDir = share.GetDirectoryReference("photos");

            //var idList = new List<Uri>();

            //foreach(var id in ids)
            //{
            //    var file = rootDir.GetBlobReference(id + ".jpg"); //TODO enum with image formats

            //    var fileSasUri = new Uri(file.StorageUri.PrimaryUri.ToString() + sasToken);

            //    idList.Add(fileSasUri);

            //}
            //return Ok(idList);
        }
    }
}
