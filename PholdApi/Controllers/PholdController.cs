using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;

namespace PholdApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PholdController : ControllerBase
    {
        private readonly ILogger<PholdController> _logger;
        private readonly IConfiguration _config;

        public PholdController(ILogger<PholdController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
        
        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<string>> GetTestAsync()
        {
            var _connString = _config.GetSection("StorageAccountConnectionString").Value;
            CloudStorageAccount storageAccount;
            if(!CloudStorageAccount.TryParse(_connString, out storageAccount))
            {
                return StatusCode(500, "Unable to connect to storage");
            }
            var fileClient = storageAccount.CreateCloudFileClient();
            var share = fileClient.GetShareReference("phold");
            string policyName = "PholdSharePolicy" + DateTime.UtcNow.Ticks;
            var sharedPolicy = new SharedAccessFilePolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(1),
                Permissions = SharedAccessFilePermissions.Read
            };

            var permissions = await share.GetPermissionsAsync();

            permissions.SharedAccessPolicies.Add(policyName, sharedPolicy);
            await share.SetPermissionsAsync(permissions);

            
            var rootDir = share.GetRootDirectoryReference();
            var sampleDir = rootDir.GetDirectoryReference("photos");
            var file = sampleDir.GetFileReference("1.jpg");

            var sasToken = file.GetSharedAccessSignature(null, policyName);
            var fileSasUri = new Uri(file.StorageUri.PrimaryUri.ToString() + sasToken);

            return Ok(fileSasUri);
        }
       
    }
}
