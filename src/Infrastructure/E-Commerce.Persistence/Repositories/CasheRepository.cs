using E_Commerce.Domain.Contracts;
using StackExchange.Redis;

namespace E_Commerce.Persistence.Repositories
{
    public class CasheRepository : ICasheRepository
    {
        private readonly IDatabase _database;
        public CasheRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<string?> GetAsync(string key)
        {
            var redisValue = await _database.StringGetAsync(key);
            return redisValue.HasValue ? redisValue.ToString() : null;
        }

        public async Task RemoveAsync(string key) => await _database.KeyDeleteAsync(key);

        public async Task SetAsync(string key, string cacheValue, TimeSpan expiration)
            => await _database.StringSetAsync(key, cacheValue, expiration);
    }
}
