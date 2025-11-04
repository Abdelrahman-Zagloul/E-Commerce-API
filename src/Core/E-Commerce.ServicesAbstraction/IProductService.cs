using E_Commerce.Shared;
using E_Commerce.Shared.DTOs.Products;
using E_Commerce.Shared.Parameters;

namespace E_Commerce.ServicesAbstraction
{
    public interface IProductService
    {
        Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams);
        Task<IEnumerable<ProductBrandDto>> GetAllBrandsAsync();
        Task<IEnumerable<ProductTypeDto>> GetAllTypesAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
    }
}
