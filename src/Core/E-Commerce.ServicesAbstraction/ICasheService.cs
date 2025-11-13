namespace E_Commerce.ServicesAbstraction
{
    public interface ICasheService
    {
        Task<string?> GetAsync(string key);
        Task SetAsync(string key, object value, TimeSpan expiration);
        Task RemoveAsync(string key);
    }
}
