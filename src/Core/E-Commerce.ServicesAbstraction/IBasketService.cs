using E_Commerce.Shared.DTOs.Baskets;

namespace E_Commerce.ServicesAbstraction
{
    public interface IBasketService
    {
        Task<BasketDto> GetBasketAsync(string id);
        Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basket);
        Task<bool> DeleteBasketAsync(string id);
    }
}
