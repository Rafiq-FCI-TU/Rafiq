using Rafiq.Application.Interfaces.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace Rafiq.Infrastructure.Services.Auth;
public class RedisCacheService(IConnectionMultiplexer redis) : IRedisCacheService
{
    private readonly IDatabase _redisDb = redis.GetDatabase();

    public async Task SetAsync(string key, object value, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(value);
        await _redisDb.StringSetAsync(key, json, expiry);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _redisDb.StringGetAsync(key);
        return value.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(value!);
    }

    public async Task RemoveAsync(string key)
    {
        await _redisDb.KeyDeleteAsync(key);
    }
}
