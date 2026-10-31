using Application.DTOs;

namespace Application.Interfaces;

public interface IProductCategory
{
    Task<List<GetProductCategoryDTO>> GetAllProductCategoriesAsync();
    Task<GetProductCategoryDTO> GetProductCategoryByIdAsync(int id);
    Task AddProductCategoryAsync(AddProductCategoryDTO productCategory);
}