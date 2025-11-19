using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.DTOs.Baskets;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    public class BasketController : ApiBaseController
    {

        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasket(string id)
        {
            var result = await _basketService.GetBasketAsync(id);
            return HandleResult<BasketDto>(result);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> AddOrUpdate([FromBody] BasketDto basketDto)
        {
            var result = await _basketService.CreateOrUpdateBasketAsync(basketDto);
            return Ok(result);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            var result = await _basketService.DeleteBasketAsync(id);
            return HandleResult(result);
        }
    }
}
