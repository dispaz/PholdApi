using PholdApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Interfaces
{
    public interface IPholdService
    {
        Task<List<PholdObject>> GetPholdObjectsWithImagesAsync();
        Task<List<BasePholdObject>> GetPholdObjectsAsync();
        Task<List<GetPhotoInfo>> GetPhotoData(int pholdId);
    }
}
