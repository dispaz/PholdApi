using PholdApi.Models.Db;
using PholdApi.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Interfaces
{
    public interface IDbService
    {
        Task<bool> FindApiKey(string apiKey, string method);
        Task<List<PholdObjectDb>> GetPholdObjectsAsync(double? latitude, double? longitude, double? radius);
        Task<List<PholdObjectDb>> GetPholdObjectsAsync();
        Task<int> AddNewPholdObjectAsync(PostPholdObject pholdObject, double radius);
        void StorePhotoInfo(PostPhotoInfo photoInfo, string filename);
        Task<bool> PholdObjectExists(int id);
        Task<List<PhotoInfoDb>> GetPhotoInfosAsync(int pholdId);
    }
}
