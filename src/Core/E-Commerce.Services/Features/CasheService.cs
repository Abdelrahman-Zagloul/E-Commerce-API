using E_Commerce.Domain.Contracts;
using E_Commerce.ServicesAbstraction;
using System.Text.Json;

namespace E_Commerce.Services.Features
{
    public class CasheService : ICasheService
    {
        private readonly ICasheRepository _casheRepository;

        public CasheService(ICasheRepository casheRepository)
        {
            _casheRepository = casheRepository;
        }

        public async Task<string?> GetAsync(string key) => await _casheRepository.GetAsync(key);

        public async Task RemoveAsync(string key) => await _casheRepository.RemoveAsync(key);

        public async Task SetAsync(string key, object value, TimeSpan expiration) => await _casheRepository.SetAsync(key, JsonSerializer.Serialize(value), expiration);
    }
}
