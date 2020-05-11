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
using PholdApi.Models;
using System.Data;
using PholdApi.Helpers;

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
                if (!string.IsNullOrEmpty(result))
                {
                    return true;
                }
                return false;
            }
        }

        public List<PholdObject> GetPholdObjects(double? latitude, double? longitude, double? radius)
        {
            var query = Sql.SqlSp.GetPholdObjects;
            return Execute(x => x.Query<PholdObject>(query, new { latitude, longitude, radius }, commandType: CommandType.StoredProcedure).ToList());
        }

        public List<PholdObject> GetPholdObjects()
        {
            return Execute(x => x.Query<PholdObject>("SELECT * FROM PholdObjects").ToList());
        }

        public List<PhotoInfo> GetPhotoInfos(int pholdId)
        {
            _logger.LogInformation($"action=get_photo_info phold_id={pholdId}");
            var query = "SELECT FileName, FromYear, ToYear FROM PholdPhotos WHERE PholdObjectID = @Id";
            return Execute(x => x.Query<PhotoInfo>(query, new { Id = pholdId })).ToList();
        }
        
        public int AddNewPholdObject(SavePholdObject pholdObject, double radius)
        {
            var query = Sql.SqlSp.AddPholdObject;
            return Execute(x => x.ExecuteScalar<int>(query, new
            {
                pholdObject.Name,
                pholdObject.Street,
                pholdObject.Latitude,
                pholdObject.Longitude,
                pholdObject.Description,
                pholdObject.AreaCode,
                radius
            }, commandType: CommandType.StoredProcedure));
        }

        public void StorePhotoInfo(PostPhotoInfo photoInfo)
        {
            var query = "INSERT INTO [dbo].[PholdPhotos] ([PholdObjectID], [FileName], [FromYear], [ToYear]) VALUES (@PholdObjectID, @FileName, @FromYear, @ToYear)";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, new { PholdObjectID = photoInfo.Id, FileName = photoInfo.Filename, FromYear = photoInfo.FromYear, ToYear = photoInfo.ToYear });
            }

        }

        private T Execute<T>(Func<IDbConnection, T> func)
        {
            var conString = _connectionString;
            using (var con = new SqlConnection(conString))
            {
                con.Open();

                return func(con);
            }
        }

        public async Task<bool> PholdObjectExists(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QuerySingleOrDefaultAsync("SELECT ID FROM PholdObjects WHERE ID = @id", new { id });
                if (result != null)
                {
                    return true;
                }
                return false;
            }
        }

    }
}
