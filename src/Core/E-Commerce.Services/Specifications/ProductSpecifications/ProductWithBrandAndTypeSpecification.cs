using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Shared.Parameters;

namespace E_Commerce.Services.Specifications.ProductSpecifications
{
    internal class ProductWithBrandAndTypeSpecification : BaseSpecification<Product, int>
    {
        //Get By Id including Brand and Type
        public ProductWithBrandAndTypeSpecification(int id)
            : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }

        // Get All with include Brand and Type
        public ProductWithBrandAndTypeSpecification(ProductQueryParams queryParams)
            : base(ProductSpecificationHelper.BuildCriteria(queryParams))
        {
            DisableTracking();
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);

            switch (queryParams.sortingOption)
            {
                case ProductSortingOption.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOption.PriceDesc:
                    AddOrderByDescending(p => p.Price);
                    break;
                case ProductSortingOption.NameDesc:
                    AddOrderByDescending(p => p.Name);
                    break;
                case ProductSortingOption.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                default:
                    //AddOrderBy(p => p.Id);
                    break;
            }

            ApplayPagination(queryParams.PageIndex, queryParams.PageSize);
        }
    }
}
