using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.Baskets;

namespace E_Commerce.ServicesAbstraction
{
    public interface IBasketService
    {
        Task<Result<BasketDto>> GetBasketAsync(string id);
        Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basket);
        Task<Result> DeleteBasketAsync(string id);
    }
}
