using E_Commerce.Shared.DTOs.Products;

namespace E_Commerce.ServicesAbstraction
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<IEnumerable<ProductBrandDto>> GetAllBrandsAsync();
        Task<IEnumerable<ProductTypeDto>> GetAllTypesAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
    }
}
