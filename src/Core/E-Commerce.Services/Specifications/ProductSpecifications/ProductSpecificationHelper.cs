using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Shared.Parameters;
using System.Linq.Expressions;

namespace E_Commerce.Services.Specifications.ProductSpecifications
{
    internal static class ProductSpecificationHelper
    {
        public static Expression<Func<Product, bool>> BuildCriteria(ProductQueryParams queryParams)
        {
            return p =>
                (!queryParams.brandId.HasValue || p.BrandId == queryParams.brandId.Value) &&
                (!queryParams.typeId.HasValue || p.TypeId == queryParams.typeId.Value) &&
                (string.IsNullOrWhiteSpace(queryParams.searchTerm) ||
                 p.Name.ToLower().Contains(queryParams.searchTerm.ToLower()));
        }
    }
}
