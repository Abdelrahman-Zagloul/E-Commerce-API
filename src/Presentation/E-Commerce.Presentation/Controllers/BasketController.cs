using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.DTOs.Baskets;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
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
            if (result is null)
                return NotFound();
            return Ok(result);
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
            await _basketService.DeleteBasketAsync(id);
            return NoContent();
        }
    }
}
