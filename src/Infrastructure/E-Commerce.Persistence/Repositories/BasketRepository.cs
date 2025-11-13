using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.BasketModule;
using StackExchange.Redis;
using System.Text.Json;

namespace E_Commerce.Persistence.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<CustomerBasket> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan timeToLive)
        {
            var jsonBasket = JsonSerializer.Serialize(basket);
            bool isCreatedOrUpdated = await _database.StringSetAsync(basket.Id, jsonBasket, timeToLive);
            return basket;
        }

        public async Task<bool> DeleteBasketAsync(string basketId) => await _database.KeyDeleteAsync(basketId);

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var jsonBasket = await _database.StringGetAsync(basketId);
            return jsonBasket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(jsonBasket!);
        }
    }
}
