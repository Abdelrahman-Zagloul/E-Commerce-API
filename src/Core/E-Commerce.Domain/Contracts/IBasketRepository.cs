using E_Commerce.Domain.Entities.BasketModule;

namespace E_Commerce.Domain.Contracts
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan timeToLive);
        Task<CustomerBasket?> GetBasketAsync(string basketId);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
