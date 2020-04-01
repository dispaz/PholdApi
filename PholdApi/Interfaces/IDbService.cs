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
        int AddNewPholdObject(PholdObject pholdObject, double radius);
        void StorePhotoInfo(int id, PhotoInfo photoInfo);
        Task<bool> PholdObjectExists(int id);
    }
}
