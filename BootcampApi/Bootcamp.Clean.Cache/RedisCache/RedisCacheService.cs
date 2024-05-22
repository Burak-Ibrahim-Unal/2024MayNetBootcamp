using Bootcamp.Clean.ApplicationService.Interfaces;
using StackExchange.Redis;
using System.Net;

namespace Bootcamp.Clean.Cache.RedisCache
{
    public class RedisCacheService() : IRedisCacheService
    {
        public ConnectionMultiplexer _connectionMultiplexer;
        public IDatabase _database;

        IDatabase IRedisCacheService.RedisDatabase { get { return _database; } set { _database = value; } }
        ConnectionMultiplexer IRedisCacheService.RedisMultiplexer { get { return _connectionMultiplexer; } set { _connectionMultiplexer = value; } }

        public RedisCacheService(string url) : this()
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(url);
            _database = _connectionMultiplexer.GetDatabase(3);
        } 
    }
}
