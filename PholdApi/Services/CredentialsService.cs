using Microsoft.Extensions.Logging;
using PholdApi.Interfaces;
using System.Threading.Tasks;

namespace PholdApi.Services
{
    public class CredentialsService : ICredentialsService
    {
        private readonly ILogger _logger;
        private readonly IDbService _dbService;
        public CredentialsService(ILogger<CredentialsService> logger, IDbService dbService)
        {
            _logger = logger;
            _dbService = dbService;
        }

        public async Task<bool> CheckApiKey(string apiKey)
        {
            return await _dbService.FindApiKey(apiKey);
        }
    }
}
