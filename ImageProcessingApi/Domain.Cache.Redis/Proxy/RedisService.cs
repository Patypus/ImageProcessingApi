using Domain.Cache.Abstractions.Dtos;
using Domain.Cache.Abstractions.Proxy;
using Domain.Cache.Redis.Configuration;
using Domain.Cache.Redis.Connection;
using Microsoft.Extensions.Options;

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

        public void AddImageToCache(ImageRequestDto requestKey, byte[] imageData)
        {
            if (_configuration.CacheEnabled)
            {
                var database = _connection.GetDatabase();
                database.StringSet(requestKey.Name, imageData);
            }
        }

        public void ClearCache()
        {
            if (_configuration.CacheEnabled)
            {
                var server = _connection.GetServer();
                server.FlushDatabase();
            }
        }

        public byte[] GetImageFromCache(ImageRequestDto requestDto)
        {
            if (!_configuration.CacheEnabled)
            {
                return null;
            }

            var database = _connection.GetDatabase();
            var imageData = database.StringGet(requestDto.Name);

            return imageData;
        }
    }
}
