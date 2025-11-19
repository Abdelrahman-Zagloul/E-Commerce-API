using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.BasketModule;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.Baskets;

namespace E_Commerce.Services.Features
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        public async Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basket)
        {
            var customerBasket = _mapper.Map<CustomerBasket>(basket);
            var result = await _basketRepository.CreateOrUpdateBasketAsync(customerBasket, TimeSpan.FromMinutes(60));
            return _mapper.Map<BasketDto>(result);
        }

        public async Task<Result> DeleteBasketAsync(string id)
        {
            var isDeleted = await _basketRepository.DeleteBasketAsync(id);
            if (!isDeleted)
                return Result.Fail(Error.NotFound(description: $"Basket with id: '{id}' not found"));

            return Result.Ok();
        }
        public async Task<Result<BasketDto>> GetBasketAsync(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            if (basket == null)
                return Error.NotFound(description: $"Basket with id: '{id}' not found"); // implicit conversion to Result<BasketDto>.Fail
            //return Result<BasketDto>.Fail(Error.NotFound(description: $"Basket with id: '{id}' not found"));

            return _mapper.Map<BasketDto>(basket);// implicit conversion to Result<basketDto>.Ok
            //return Result<BasketDto>.Ok(_mapper.Map<BasketDto>(basket));
        }
    }
}
