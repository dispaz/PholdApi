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

        private double _radius;

        public PholdController(ILogger<PholdController> logger, IConfiguration config, IPholdStorageService storageService, IDbService dbService)
        {
            _logger = logger;
            _config = config;
            _storageService = storageService;
            _dbService = dbService;

            _radius = _config.GetSection("ApplicationValues").GetValue<double>("BlockAddingRadius");
        }

        /// <summary>
        /// Get phold objects
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of phold objects</returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("GetPholdObjects")]
        public ActionResult<List<PholdObject>> GetPholdObjects(double? latitude, double? longitude, double? radius)
        {
            try
            {
                return Ok(_dbService.GetPholdObjects(latitude, longitude, radius));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
        public ActionResult<int> CreateNewObject([FromForm]SavePholdObject pholdObject)
        {            
            if(_radius <= 0)
                return StatusCode(404, "Radius is invalid");
            
            try
            {
                return Ok(_dbService.AddNewPholdObject(pholdObject, _radius));
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
            
        }

        /// <summary>
        /// Upload photo to Azure storage and store photo info in database
        /// </summary>
        /// <param name="image"></param>
        /// <param name="years"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpPost]
        [Route("UploadPhoto")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<string>> UploadPholdPhoto([SwaggerFile]IFormFile image, [FromForm]string years, [FromForm]int id)
        {
            try
            {
                if (!(Path.GetExtension(image.FileName).Contains(".jpg") ||
                    Path.GetExtension(image.FileName).Contains(".png")))
                    return StatusCode(404, "File exstension is invalid. Only .jpg or .png");
                
                if(!(await _dbService.PholdObjectExists(id)))
                    return StatusCode(404, "Phold object with that ID does not exist");

                var uploadedFileName = await _storageService.UploadPhotoAsync(id, image);

                _dbService.StorePhotoInfo(id, new PhotoInfo(uploadedFileName, years));
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
