using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Shared.Parameters;

namespace E_Commerce.Services.Specifications.ProductSpecifications
{
    internal class ProductCountSpecifications : BaseSpecification<Product, int>
    {
        public ProductCountSpecifications(ProductQueryParams queryParams)
            : base(ProductSpecificationHelper.BuildCriteria(queryParams))
        {
        }
    }
}
