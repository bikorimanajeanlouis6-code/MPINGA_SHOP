using Application.DTOs;
using Application.Interfaces;

namespace Application.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IProduct _productRepository;

        public ProductService(IProduct productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<List<GetProductDTO>> GetAllProductAsync() =>
            _productRepository.GetAllProductAsync();

        public Task<GetProductDTO> GetProductByIdAsync(int id) =>
            _productRepository.GetProductByIdAsync(id);

        public Task AddProductAsync(AddProductDTO product) =>
            _productRepository.AddProductAsync(product);
    }
}