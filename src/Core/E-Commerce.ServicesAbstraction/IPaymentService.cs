using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.Baskets;

namespace E_Commerce.ServicesAbstraction
{
    public interface IPaymentService
    {
        Task<Result<BasketDto>> CreateOrUpdatePaymentIntent(string basketId);
        Task UpdateOrderStatus(string json, string signatureHeader);
    }
}
