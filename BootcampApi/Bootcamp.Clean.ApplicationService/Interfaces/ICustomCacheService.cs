using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Clean.ApplicationService.Interfaces
{
    public interface ICustomCacheService
    {
        Task<string> GetValueAsync(string key);
        Task SetValueAsync(string  key, string value);
        Task<bool> DeleteKeyAsync(string key);
        Task<bool> KeyExistsAsync(string key);
        Task<bool> KeyDeleteAsync(string key);
        Task<long> ListLeftPushAsync(string key, string value);
        Task<long> ListRightPushAsync(string key, string value);
        Task<string> ListGetByIndexAsync(string key);

    }
}
