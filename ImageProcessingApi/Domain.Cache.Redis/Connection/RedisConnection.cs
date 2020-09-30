using Domain.Cache.Redis.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

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
                _connection = ConnectionMultiplexer.Connect($"{_configuration.HostName}:{_configuration.PortNumber}");
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

        public IServer GetServer(string serverName)
        {
            return _connection.GetServer(serverName);
        }
    }
}
