using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.OrderModule;
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
                if (basket.PaymentIntendId == null)
                {
                    var paymentResult = await service.CreateAsync(new PaymentIntentCreateOptions
                    {
                        Amount = totalPriceInCent,
                        Currency = "usd",
                        PaymentMethodTypes = ["card"]
                    });

                    basket.PaymentIntendId = paymentResult.Id;
                    basket.ClientSecret = paymentResult.ClientSecret;

                }
                else
                {
                    await service.UpdateAsync(basket.PaymentIntendId, new PaymentIntentUpdateOptions
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
    }
}
