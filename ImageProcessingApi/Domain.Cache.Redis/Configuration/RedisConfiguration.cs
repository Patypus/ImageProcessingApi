using System.Collections.Generic;

namespace Domain.Cache.Redis.Configuration
{
    public class RedisConfiguration
    {
        public bool CacheEnabled { get; set; }
        public bool AllowAdmin { get; set; }
        public List<EndpointConfiguration> Endpoints { get; set; }
    }
}
