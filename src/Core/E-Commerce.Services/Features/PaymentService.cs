using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Services.Specifications.OrderSpecifications;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.Baskets;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = E_Commerce.Domain.Entities.ProductModule.Product;

namespace E_Commerce.Services.Features
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<Result<BasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            //get basket from repository
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket == null)
                return Error.NotFound("Basket Not Found");

            //calculate shipping price
            if (!basket.DeliveryMethodId.HasValue)
                return Error.NotFound($"Delivery Method Not Found");

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value);
            if (deliveryMethod == null)
                return Error.NotFound($"Delivery Method Not Found", $"Delivery Method with id {basket.DeliveryMethodId.Value} Not Found");
            basket.ShippingPrice = deliveryMethod.Cost;


            // get total price
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(item.Id);
                if (product == null)
                    return Error.NotFound($"Product with id {item.Id} Not Found");

                item.Price = product.Price;
                item.Name = product.Name;
                item.PictureUrl = product.PictureUrl;
            }
            long totalPriceInCent = (long)(basket.Items.Sum(i => i.Quantity * i.Price) + basket.ShippingPrice) * 100;


            //create or update payment intent with stripe
            try
            {
                StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
                var service = new PaymentIntentService();

                // create payment intent    
                if (basket.PaymentIntentId == null)
                {
                    var paymentResult = await service.CreateAsync(new PaymentIntentCreateOptions
                    {
                        Amount = totalPriceInCent,
                        Currency = "usd",
                        PaymentMethodTypes = ["card"]
                    });

                    basket.PaymentIntentId = paymentResult.Id;
                    basket.ClientSecret = paymentResult.ClientSecret;

                }
                else
                {
                    await service.UpdateAsync(basket.PaymentIntentId, new PaymentIntentUpdateOptions
                    {
                        Amount = totalPriceInCent,
                    });
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("Payment Intent Error", ex.Message);
            }


            //save changes to repository
            await _basketRepository.CreateOrUpdateBasketAsync(basket, TimeSpan.FromDays(7));
            return _mapper.Map<BasketDto>(basket);
        }

        public async Task UpdateOrderStatus(string json, string signatureHeader)
        {
            try
            {
                string endpointSecret = _configuration["Stripe:EndpointSecret"]!;
                var stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, endpointSecret);

                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                if (paymentIntent == null)
                    return;

                if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                    await UpdateOrderPaymentStatus(paymentIntent.Id, OrderStatus.PaymentReceived);
                else
                    await UpdateOrderPaymentStatus(paymentIntent.Id, OrderStatus.PaymentFailed);
            }
            catch (StripeException ex)
            {
                return;
            }
        }
        private async Task UpdateOrderPaymentStatus(string paymentIntentId, OrderStatus orderStatus)
        {
            var spec = new OrderPaymentSpecification(paymentIntentId);
            var order = await _unitOfWork.Repository<Order, Guid>().GetByIdAsync(spec);

            if (order == null)
                return;

            order.status = orderStatus;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
