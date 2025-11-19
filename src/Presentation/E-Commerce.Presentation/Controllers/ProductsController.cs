using E_Commerce.Presentation.Attributes;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.DTOs.Products;
using E_Commerce.Shared.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    public class ProductsController : ApiBaseController
    {

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [RedisCashe(10)]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParams queryParams)
        {
            var products = await _productService.GetAllProductsAsync(queryParams);
            return Ok(products);
        }

        [HttpGet("brand")]
        [RedisCashe(10)]
        public async Task<IActionResult> GetAllBrands()
        {
            var brands = await _productService.GetAllBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("type")]
        [RedisCashe(10)]
        public async Task<IActionResult> GetAllTypes()
        {
            var types = await _productService.GetAllTypesAsync();
            return Ok(types);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            return HandleResult<ProductDto>(result);
        }
    }
}