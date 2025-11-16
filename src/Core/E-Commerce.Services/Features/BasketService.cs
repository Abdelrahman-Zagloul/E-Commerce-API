using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.BasketModule;
using E_Commerce.Services.Exceptions;
using E_Commerce.ServicesAbstraction;
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

        public async Task<bool> DeleteBasketAsync(string id)
        {
            var isDeleted = await _basketRepository.DeleteBasketAsync(id);
            if (!isDeleted)
                throw new BasketNotFoundException(id);
            return true;
        }
        public async Task<BasketDto> GetBasketAsync(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            if (basket == null)
                throw new BasketNotFoundException(id);

            var basketDto = _mapper.Map<BasketDto>(basket);
            return basketDto;
        }
    }
}
