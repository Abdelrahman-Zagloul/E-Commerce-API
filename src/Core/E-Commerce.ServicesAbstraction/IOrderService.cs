using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.Orders;

namespace E_Commerce.ServicesAbstraction
{
    public interface IOrderService
    {
        Task<Result<OrderDto>> CreateOrderAsync(string email, CreateOrderDto dto);
        Task<Result<List<OrderDto>>> GetOrdersForUserAsync(string email);
        Task<Result<OrderDto>> GetOrderByIdAsync(string email, Guid orderId);
        Task<Result<List<DeliveryMethodDto>>> GetDeliveryMethod();
    }
}
