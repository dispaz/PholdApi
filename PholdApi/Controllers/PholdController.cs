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
using Microsoft.AspNetCore.Http;
using PholdApi.Filters;
using PholdApi.Models;
using NSwag.Annotations;

namespace PholdApi.Controllers
{
    [ServiceFilter(typeof(CredentialsFilter))]
    [ApiController]
    [Route("[controller]")]
    public class PholdController : ControllerBase
    {
        private readonly ILogger<PholdController> _logger;
        private readonly IConfiguration _config;
        private readonly IPholdStorageService _storageService;
        private readonly IDbService _dbService;

        public PholdController(ILogger<PholdController> logger, IConfiguration config, IPholdStorageService storageService, IDbService dbService)
        {
            _logger = logger;
            _config = config;
            _storageService = storageService;
            _dbService = dbService;
        }

        /// <summary>
        /// Get urls to download images
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of urls to images</returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("ObjectImagesById")]
        public async Task<ActionResult<List<Uri>>> GetUrlsForObjectImageByIdsAsync(int id)
        {
            //TODO logging
            try
            {
                var images = await _storageService.GetImagesAsync(id);

                if (images.Count > 0)
                    return Ok(images);
                else
                    return StatusCode(404, "Not found images for that id");

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Route("CreateNewObject")]
        public ActionResult<int> CreateNewObject([FromBody]PholdObject pholdObject)
        {
            //_dbService.AddOrUpdatePholdObject()   
            return StatusCodes.Status200OK;
        }

        [HttpPost]
        [Route("UploadPhoto")]
        [Consumes("multipart/form-data")]
        public ActionResult<int> UploadPholdPhoto([SwaggerFile]IFormFile images, [FromForm]string years)
        {
            return Ok();
        }
    }
}
