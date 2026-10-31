using Application.DTOs;
using Application.Interfaces;

namespace Application.Services.ProductCategoryServices
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategory _productCategoryRepository;

        public ProductCategoryService(IProductCategory productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public Task<List<GetProductCategoryDTO>> GetAllProductCategoriesAsync() =>
            _productCategoryRepository.GetAllProductCategoriesAsync();

        public Task<GetProductCategoryDTO> GetProductCategoryByIdAsync(int id) =>
            _productCategoryRepository.GetProductCategoryByIdAsync(id);

        public Task AddProductCategoryAsync(AddProductCategoryDTO productCategory) =>
            _productCategoryRepository.AddProductCategoryAsync(productCategory);
    }
}