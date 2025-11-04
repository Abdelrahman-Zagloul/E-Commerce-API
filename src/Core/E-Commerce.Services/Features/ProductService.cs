using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Services.Specifications.ProductSpecifications;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared;
using E_Commerce.Shared.DTOs.Products;
using E_Commerce.Shared.Parameters;

namespace E_Commerce.Services.Features
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductBrandDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.Repository<ProductBrand, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<ProductBrandDto>>(brands);
        }

        public async Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var productRepo = _unitOfWork.Repository<Product, int>();

            var spec = new ProductWithBrandAndTypeSpecification(queryParams);
            var CountSpec = new ProductCountSpecifications(queryParams);

            var products = await productRepo.GetAllAsync(spec);
            var TotalCount = await productRepo.CountAsync(CountSpec);
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            return new PaginatedResult<ProductDto>(queryParams.PageIndex, productDtos.Count(), TotalCount, productDtos);
        }

        public async Task<IEnumerable<ProductTypeDto>> GetAllTypesAsync()
        {
            var products = await _unitOfWork.Repository<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<ProductTypeDto>>(products);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecification(id);
            var products = await _unitOfWork.Repository<Product, int>().GetByIdAsync(spec);
            return _mapper.Map<ProductDto>(products);
        }

    }
}
