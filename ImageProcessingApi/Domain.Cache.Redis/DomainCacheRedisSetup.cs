using Domain.Cache.Abstractions.Proxy;
using Domain.Cache.Redis.Connection;
using Domain.Cache.Redis.Proxy;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Cache.Redis
{
    public class DomainCacheRedisSetup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICacheService, RedisService>();
        }
    }
}
