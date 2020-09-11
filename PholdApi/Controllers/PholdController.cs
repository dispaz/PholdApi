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
        private readonly IPholdService _pholdService;
        private double _radius;

        public PholdController(ILogger<PholdController> logger, IConfiguration config, IPholdStorageService storageService, IDbService dbService, IPholdService pholdService)
        {
            _logger = logger;
            _config = config;
            _storageService = storageService;
            _dbService = dbService;
            _pholdService = pholdService;

            _radius = _config.GetSection("ApplicationValues").GetValue<double>("BlockAddingRadius");
        }
        
        /// <summary>
        /// Get all phold objects with images
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("get/fullpholds")]
        public async Task<ActionResult<List<PholdObject>>> GetPholdObjectsWithImagesAsync()
        {
            try
            {
                var pholds = await _pholdService.GetPholdObjectsWithImagesAsync();
                return Ok(pholds);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get all phold objects
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("get/pholds")]
        public async Task<ActionResult<List<BasePholdObject>>> GetPholdObjectsAsync()
        {
            try
            {
                var pholds = await _pholdService.GetPholdObjectsAsync();
                return Ok(pholds);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get all images for phold id
        /// </summary>
        /// <param name="id">pholdId</param>
        /// <returns>List of photo info with urls</returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("get/phold/{id}/photos")]
        public async Task<ActionResult<List<GetPhotoInfo>>> GetPholdObjectsAsync(int id)
        {
            try
            {
                var photoInfos = await _pholdService.GetPhotoData(id);
                return Ok(photoInfos);
            }
            catch (Exception ex)
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
        [Route("get/phold/images")]
        public async Task<ActionResult<List<string>>> GetUrlsForObjectImageByIdsAsync(int id)
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
        [Route("post/phold")]
        public ActionResult<int> CreateNewObject([FromForm]SavePholdObject pholdObject)
        {            
            if(_radius <= 0)
                return StatusCode(404, "Radius is invalid");
            
            try
            {
                return Ok(_dbService.AddNewPholdObjectAsync(pholdObject, _radius));
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
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpPost]
        [Route("post/photoinfo")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<string>> UploadPholdPhoto([SwaggerFile]IFormFile image, [FromForm]PostPhotoInfo photoInfo)
        {
            try
            {
                if (!(Path.GetExtension(image.FileName).Contains(".jpg") ||
                    Path.GetExtension(image.FileName).Contains(".png")))
                    return StatusCode(400, "File exstension is invalid. Only .jpg or .png");
                
                if(!(await _dbService.PholdObjectExists(photoInfo.Id)))
                    return StatusCode(404, "Phold object with that ID does not exist");

                var uploadedFileName = await _storageService.UploadPhotoAsync(photoInfo.Id, image);

                _dbService.StorePhotoInfo(photoInfo);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
