using Application.DTOs;

namespace Application.Services.ProductCategoryServices
{
    public interface IProductCategoryService
    {
           Task <List<GetProductCategoryDTO>>GetAllProductCategoriesAsync();
           Task AddProductCategoryAsync(AddProductCategoryDTO productCategory); 
           Task <GetProductCategoryDTO> GetProductCategoryByIdAsync(int id);
       
    }
}