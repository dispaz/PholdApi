using PholdApi.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Interfaces
{
    public interface IPholdService
    {
        Task<List<GetPholdObject>> GetPholdObjectsWithImagesAsync();
        Task<List<GetPholdObject>> GetPholdObjectsAsync();
        Task<List<GetPhotoInfo>> GetPhotoData(int pholdId);
    }
}
