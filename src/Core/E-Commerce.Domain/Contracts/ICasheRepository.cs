namespace E_Commerce.Domain.Contracts
{
    public interface ICasheRepository
    {
        Task SetAsync(string key, string cacheValue, TimeSpan expiration);
        Task<string?> GetAsync(string key);
        Task RemoveAsync(string key);
    }
}
