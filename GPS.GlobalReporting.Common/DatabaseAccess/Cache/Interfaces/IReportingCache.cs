namespace GPS.GlobalReporting.Common.DatabaseAccess.Cache.Interfaces;

public interface IReportingCache
{
    void SetCache<T>(string keyName, T entry);
    ValueTask<T> GetCache<T>(string keyName, Func<Task<T>> fallback);
    void ClearCache(string keyName);
}