using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using PholdApi.Interfaces;

namespace PholdApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PholdController : ControllerBase
    {
        private readonly ILogger<PholdController> _logger;
        private readonly IConfiguration _config;
        private readonly IPholdStorageService _storageService;

        public PholdController(ILogger<PholdController> logger, IConfiguration config, IPholdStorageService storageService)
        {
            _logger = logger;
            _config = config;
            _storageService = storageService;
        }

        /// <summary>
        /// Get urls to download images
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>List of urls to images</returns>
        [HttpGet]
        [Route("ImagesByIds")]
        public ActionResult<List<Uri>> GetUrlsForObjectImageByIds([FromHeader] int [] ids)
        {
            //TODO logging
            return Ok(_storageService.GetImages(ids));            
        }

    }
}
