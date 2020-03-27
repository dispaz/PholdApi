using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PholdApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace PholdApi.Services
{
    public class DbService : IDbService
    {
        private readonly string _connectionString;
        private readonly ILogger<DbService> _logger;

        public DbService(ILogger<DbService> logger, IConfiguration config)
        {
            _logger = logger;
            _connectionString = config.GetConnectionString("Test_PholdDb");
        }
        public async Task<bool> FindApiKey(string apiKey)
        {
            _logger.LogInformation($"action=find_api_key api_key={apiKey}");
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QuerySingleOrDefaultAsync<string>("SELECT Permissions FROM Credentials WHERE ApiKey = @apiKey", new { apiKey });
                if(!string.IsNullOrEmpty(result))
                {
                    return true;
                }
                return false;
            }
        }
    }
}
