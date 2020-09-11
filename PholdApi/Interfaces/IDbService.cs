using PholdApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Interfaces
{
    public interface IDbService
    {
        Task<bool> FindApiKey(string apiKey);
        Task<List<PholdObject>> GetPholdObjectsAsync(double? latitude, double? longitude, double? radius);
        Task<List<BasePholdObject>> GetPholdObjectsAsync();
        Task<int> AddNewPholdObjectAsync(SavePholdObject pholdObject, double radius);
        void StorePhotoInfo(PostPhotoInfo photoInfo);
        Task<bool> PholdObjectExists(int id);
        Task<List<PhotoInfo>> GetPhotoInfosAsync(int pholdId);
    }
}
