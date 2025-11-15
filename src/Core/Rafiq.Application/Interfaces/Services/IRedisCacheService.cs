namespace Rafiq.Application.Interfaces.Services;
public interface IRedisCacheService
{
    Task SetAsync(string key, object value, TimeSpan? expiry = null);
    Task<T?> GetAsync<T>(string key);
    Task RemoveAsync(string key);
}
