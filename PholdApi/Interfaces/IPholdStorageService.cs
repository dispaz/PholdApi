using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Interfaces
{
    public interface IPholdStorageService
    {
        Task<List<Uri>> GetImagesAsync(int id);
        


    }
}
