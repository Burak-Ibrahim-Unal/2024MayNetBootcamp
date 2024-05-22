using StackExchange.Redis;

namespace Bootcamp.Clean.ApplicationService.Interfaces
{
    public interface IRedisCacheService
    {
        public IDatabase RedisDatabase { get; set; }
        public ConnectionMultiplexer RedisMultiplexer { get; set; }
    }
}
