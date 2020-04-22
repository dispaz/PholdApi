using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Interfaces
{
    public interface IPholdStorageService
    {
        Task<List<string>> GetImagesAsync(int id);
        Task<string> UploadPhotoAsync(int id, IFormFile file);
    }
}
