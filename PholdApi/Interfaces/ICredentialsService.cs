using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Interfaces
{
    public interface ICredentialsService
    {
        Task<bool> CheckApiKey(string apiKey);
    }
}
