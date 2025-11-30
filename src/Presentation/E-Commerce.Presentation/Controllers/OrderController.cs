using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.DTOs.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Presentation.Controllers
{
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderByIdAsync(Guid orderId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdAsync(userEmail!, orderId);
            return HandleResult(order);
        }

        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetAllOrdersForCurrentUserAsync()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrdersForUserAsync(userEmail!);
            return HandleResult(order);
        }
        [HttpGet("delivery-methods")]
        [Authorize]
        public async Task<IActionResult> GetAllDeliveryMethod()
        {
            var order = await _orderService.GetDeliveryMethod();
            return HandleResult(order);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrderAsync(CreateOrderDto dto)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.CreateOrderAsync(userEmail!, dto);
            return HandleResult(order);
        }


    }
}


