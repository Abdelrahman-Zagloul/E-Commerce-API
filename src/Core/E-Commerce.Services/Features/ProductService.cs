using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.ServicesAbstraction;
using E_Commerce.Shared.DTOs.Products;

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

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Repository<Product, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<ProductTypeDto>> GetAllTypesAsync()
        {
            var products = await _unitOfWork.Repository<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<ProductTypeDto>>(products);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var products = await _unitOfWork.Repository<Product, int>().GetByIdAsync(id);
            return _mapper.Map<ProductDto>(products);
        }
    }
}
