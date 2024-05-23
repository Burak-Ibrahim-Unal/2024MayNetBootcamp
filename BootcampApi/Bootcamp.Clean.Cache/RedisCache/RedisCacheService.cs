using Bootcamp.Clean.ApplicationService.Interfaces;
using StackExchange.Redis;
using System.Net;

namespace Bootcamp.Clean.Cache.RedisCache
{
    public class RedisCacheService() : ICustomCacheService
    {
        public ConnectionMultiplexer _connectionMultiplexer;
        public IDatabase _database;

        public RedisCacheService(string url) : this()
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(url);
            _connectionMultiplexer.ConnectionFailed += ConnectionMultiplexerConnectionFailed;
            _database = _connectionMultiplexer.GetDatabase(3);
        }

        public async Task<bool> DeleteKeyAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        public async Task<string> GetValueAsync(string key)
        {
            return await _database.StringGetAsync(key);
        }

        public async Task<bool> KeyDeleteAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        public async Task<bool> KeyExistsAsync(string key)
        {
            return await _database.KeyExistsAsync(key);
        }

        public async Task SetValueAsync(string key, string value)
        {
            await _database.StringSetAsync(key, value);
        }

        private void ConnectionMultiplexerConnectionFailed(object? sender, ConnectionFailedEventArgs e)
        {
            // When Connection failed or problem occured, send sms / email etc... Do here !!!
        }

    }
}
