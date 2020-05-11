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
        List<PholdObject> GetPholdObjects(double? latitude, double? longitude, double? radius);
        List<PholdObject> GetPholdObjects();
        int AddNewPholdObject(SavePholdObject pholdObject, double radius);
        void StorePhotoInfo(PostPhotoInfo photoInfo);
        Task<bool> PholdObjectExists(int id);
        List<PhotoInfo> GetPhotoInfos(int pholdId);
    }
}
