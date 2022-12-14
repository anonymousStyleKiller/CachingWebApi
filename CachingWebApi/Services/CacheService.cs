using System.Text.Json;
using CachingWebApi.Interfaces;
using StackExchange.Redis;

namespace CachingWebApi.Services;

public class CacheService : ICacheService
{
    private readonly IDatabase _cacheDb;

    public CacheService()
    {
  
        var redis = ConnectionMultiplexer.Connect("localhost:6379");
        _cacheDb = redis.GetDatabase();
    }

    public IEnumerable<T> GetData<T>(string key)
    {
        var value = _cacheDb.StringGet(key);
        return !string.IsNullOrWhiteSpace(value) ? JsonSerializer.Deserialize<IEnumerable<T>>(value) : default;
    }

    public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
        return _cacheDb.StringSet(key, JsonSerializer.Serialize(value));
    }

    public object RemoveData(string key)
    {
        var _exist = _cacheDb.KeyExists(key);
        if (_exist)
            return _cacheDb.KeyDelete(key);
        return false;
    }
}