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

        int AddOrUpdatePholdObject(PholdObject pholdObject, float radius, List<Tuple<string, string>> photosInfo);
    }
}
