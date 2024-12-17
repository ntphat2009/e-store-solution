using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.ApiService.Services.Interfaces
{
    public interface IRedisService
    {
        Task<string> GetCacheAsync(string key);
        Task SetCacheAsync(string key, object data, TimeSpan expiration);
        Task RemoveCacheAsync(string key);
        Task RemovePatternAsync(string pattern);
    }
}
