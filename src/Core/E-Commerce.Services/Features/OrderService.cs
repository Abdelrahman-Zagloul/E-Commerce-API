using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.BasketModule;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Services.Specifications.OrderSpecifications;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.Orders;

namespace E_Commerce.Services.Features
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepository;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _basketRepository = basketRepository;
        }

        public async Task<Result<OrderDto>> CreateOrderAsync(string email, CreateOrderDto dto)
        {
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(dto.DeliveryMethodId);
            if (deliveryMethod == null)
                return Result<OrderDto>.Fail(Error.NotFound("Delivery method not found", $"Delivery method with id:{dto.DeliveryMethodId} not found"));

            var basket = await _basketRepository.GetBasketAsync(dto.BasketId);
            if (basket == null)
                return Result<OrderDto>.Fail(Error.NotFound("Basket not found", $"Basket with id:{dto.BasketId} not found"));

            Order order = await CreateOrder(email, dto, deliveryMethod, basket);

            await _unitOfWork.Repository<Order, Guid>().AddAsync(order);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
                return Result<OrderDto>.Fail(Error.Failure("Order creation failed", "Failed to create order"));

            await _basketRepository.DeleteBasketAsync(dto.BasketId);
            return Result<OrderDto>.Ok(_mapper.Map<OrderDto>(order));
        }

        private async Task<Order> CreateOrder(string email, CreateOrderDto dto, DeliveryMethod deliveryMethod, CustomerBasket basket)
        {
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productFromDb = await _unitOfWork.Repository<Product, int>().GetByIdAsync(item.Id);
                if (productFromDb == null) continue;

                var orderItem = new OrderItem
                {
                    Product = new ProductItemOrdered
                    {
                        ProductId = productFromDb.Id,
                        ProductName = productFromDb.Name,
                        PictureUrl = productFromDb.PictureUrl
                    },
                    Price = productFromDb.Price,
                    Quantity = item.Quantity
                };
                orderItems.Add(orderItem);
            }
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);
            var order = new Order
            {
                UserEmail = email,
                ShippingAddress = _mapper.Map<OrderAddress>(dto.ShippingAddress),
                DeliveryMethodId = deliveryMethod.Id,
                DeliveryMethod = deliveryMethod,
                SubTotal = subtotal,
                Items = orderItems,
            };
            return order;
        }

        public async Task<Result<List<DeliveryMethodDto>>> GetDeliveryMethod()
        {
            var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync();
            if (!deliveryMethods.Any())
                return Result<List<DeliveryMethodDto>>.Fail(Error.NotFound(description: "No delivery methods found."));

            var deliveryMethodsDto = _mapper.Map<List<DeliveryMethodDto>>(deliveryMethods);
            return Result<List<DeliveryMethodDto>>.Ok(deliveryMethodsDto);
        }


        public async Task<Result<OrderDto>> GetOrderByIdAsync(string email, Guid orderId)
        {
            var orderSpec = new OrderSpecification(orderId, email);
            var order = await _unitOfWork.Repository<Order, Guid>().GetByIdAsync(orderSpec);
            if (order == null)
                return Result<OrderDto>.Fail(Error.NotFound("Order not found", $"Order with id:{orderId} not found for this user"));
            return Result<OrderDto>.Ok(_mapper.Map<OrderDto>(order));

        }

        public async Task<Result<List<OrderDto>>> GetOrdersForUserAsync(string email)
        {
            var orderSpec = new OrderSpecification(email);
            var orders = await _unitOfWork.Repository<Order, Guid>().GetAllAsync(orderSpec);
            if (!orders.Any())
                return Result<List<OrderDto>>.Fail(Error.NotFound("No Orders Found", "No Orders Found for this user"));
            return Result<List<OrderDto>>.Ok(_mapper.Map<List<OrderDto>>(orders));
        }
    }
}
