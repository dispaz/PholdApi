using PholdApi.Interfaces;
using PholdApi.Models.Db;
using PholdApi.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PholdApi.Services
{
    public class PholdService : IPholdService
    {
        IDbService _dbService;
        IPholdStorageService _storageService;

        public PholdService(IDbService dbService, IPholdStorageService storageService)
        {
            _dbService = dbService;
            _storageService = storageService;
        }

        public async Task<List<GetPholdObject>> GetPholdObjectsAsync()
        {
            var pholds = await _dbService.GetPholdObjectsAsync();
            return pholds.Select(x => new GetPholdObject(x)).ToList();
        }

        public async Task<List<GetPholdObject>> GetPholdObjectsWithImagesAsync()
        {
            var pholds = await _dbService.GetPholdObjectsAsync();
            var result = new List<GetPholdObject>();

            foreach (var item in pholds)
            {
                var phold = new GetPholdObject(item, await GetPhotoData(item.Id));
                result.Add(phold);
            }

            return result;
        }

        public async Task<List<GetPhotoInfo>> GetPhotoData(int pholdId)
        {
            var photoData = await _dbService.GetPhotoInfosAsync(pholdId);
            var pholdImagesUrls = await _storageService.GetImagesAsync(pholdId);
            var photoInfo = (from url in pholdImagesUrls
                             let fileName = HttpUtility.UrlDecode(url.Segments.Last())
                             from item in photoData
                             where item.Filename == fileName
                             select new GetPhotoInfo() { Id = item.Id, PholdObjectId = item.PholdObjectId, FromYear = item.FromYear, ToYear = item.ToYear, ImageUrl = url.ToString() }
                             ).ToList();
            return photoInfo;
                                          
        }
    }
}
