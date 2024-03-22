using GPS.GlobalReporting.Common.DatabaseAccess.Cache.Interfaces;
using GPS.GlobalReporting.Common.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace GPS.GlobalReporting.Common.DatabaseAccess.Cache.Concrete;

public class ReportingCache : IReportingCache
{
    private readonly IMemoryCache _memoryCache;
    private readonly IReportingConfiguration _config;

    public ReportingCache(IMemoryCache memoryCache, IReportingConfiguration config)
    {
        _memoryCache = memoryCache;
        _config = config;
    }

    public void SetCache<T>(string keyName, T entry)
    {
        //Add to cache, read timeout in minutes from config
        var cacheExpiration = _config.GetValue<int>("CacheExpirationInMinutes");
        
        _memoryCache.Set(keyName, entry, TimeSpan.FromMinutes(cacheExpiration));
    }

    public async ValueTask<T> GetCache<T>(string keyName, Func<Task<T>> fallback)
    {
        if (_memoryCache.TryGetValue(keyName, out T result))
        {
            return result;
        }

        result = await fallback();
        SetCache(keyName, result);
        return result;
    }

    public void ClearCache(string keyName)
    {
        _memoryCache.Remove(keyName);
    }
}