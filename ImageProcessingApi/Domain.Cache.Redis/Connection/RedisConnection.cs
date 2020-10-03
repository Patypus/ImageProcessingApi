using Domain.Cache.Redis.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Domain.Cache.Redis.Connection
{
    public class RedisConnection
    {
        private readonly RedisConfiguration _configuration;
        private readonly ConnectionMultiplexer _connection;
        private static RedisConnection _instance;

        private RedisConnection(RedisConfiguration configuration)
        {
            _configuration = configuration;

            if (_configuration.CacheEnabled)
            {
                var hosts = configuration.Endpoints.Select(endpoint => $"{endpoint.HostName}:{endpoint.PortNumber}");
                var hostsString = String.Join(",", hosts);
                _connection = ConnectionMultiplexer.Connect($"{hostsString},allowAdmin={_configuration.AllowAdmin}");
            }
        }

        public static RedisConnection GetConnection(RedisConfiguration configuration)
        {
            if (_instance == null)
            {
                _instance = new RedisConnection(configuration);
            }

            return _instance;
        }

        public IDatabase GetDatabase()
        {
            return _connection.GetDatabase();
        }

        public IServer GetServer(EndPoint endpoint)
        {
            return _connection.GetServer(endpoint);
        }

        public IEnumerable<EndPoint> GetEndPoints()
        {
            return _connection.GetEndPoints();
        }
    }
}
