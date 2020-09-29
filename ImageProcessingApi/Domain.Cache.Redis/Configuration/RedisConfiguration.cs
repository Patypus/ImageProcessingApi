namespace Domain.Cache.Redis.Configuration
{
    public class RedisConfiguration
    {
        public bool CacheEnabled { get; set; }
        public string HostName { get; set; }
        public string PortNumber { get; set; }
    }
}
