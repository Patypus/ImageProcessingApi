using Domain.Cache.Abstractions.Dtos;
using Domain.Cache.Abstractions.Proxy;
using Domain.Cache.Redis.Configuration;
using Domain.Cache.Redis.Connection;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Cache.Redis.Proxy
{
    public class RedisService : ICacheService
    {
        private readonly RedisConnection _connection;
        private readonly RedisConfiguration _configuration;

        public RedisService(IOptions<RedisConfiguration> configuration)
        {
            _configuration = configuration.Value;
            _connection = RedisConnection.GetConnection(_configuration);
        }

        public async Task AddImageToCacheAsync(CacheRequestDto requestDto, byte[] imageData)
        {
            if (_configuration.CacheEnabled)
            {
                var cacheKey = GenerateKeyFromRequest(requestDto);
                var database = _connection.GetDatabase();
                await database.StringSetAsync(cacheKey, imageData);
            }
        }

        public async Task ClearCacheAsync()
        {
            if (_configuration.CacheEnabled)
            {
                var flushTasks = _connection.GetEndPoints()
                    .Select(endpoint => Task.Run(() => 
                    {
                        var server = _connection.GetServer(endpoint);
                        server.FlushDatabaseAsync();
                    }))
                    .ToList();

                await Task.WhenAll(flushTasks);
            }
        }

        public async Task<byte[]> GetImageFromCacheAsync(CacheRequestDto requestDto)
        {
            if (!_configuration.CacheEnabled)
            {
                return null;
            }
            var cacheKey = GenerateKeyFromRequest(requestDto);
            var database = _connection.GetDatabase();
            var imageData = await database.StringGetAsync(cacheKey);

            return imageData;
        }

        private string GenerateKeyFromRequest(CacheRequestDto requestDto)
        {
            var key = $"name:{requestDto.Name}:format:{requestDto.FileType}:resolution:{requestDto.Resolution}:watermark:{requestDto.Watermark}:colour:{requestDto.BackgroundColour}";
            
            return key;
        }
    }
}
