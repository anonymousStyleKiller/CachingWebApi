namespace CachingWebApi.Interfaces;

public interface ICacheService
{
    IEnumerable<T> GetData<T>(string key);
    bool SetData<T>(string key, T value, DateTimeOffset expirationTime);
    object RemoveData(string key);
}