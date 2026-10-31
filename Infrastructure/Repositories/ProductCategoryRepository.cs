using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductCategoryRepository : IProductCategory
    {
        private readonly ApplicationDbContext _dbcontext;

        public ProductCategoryRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<GetProductCategoryDTO>> GetAllProductCategoriesAsync()
        {
            var entities = await _dbcontext.ProductCategories.AsNoTracking().ToListAsync();
            return entities.Select(ToDto).ToList();
        }

        public async Task<GetProductCategoryDTO> GetProductCategoryByIdAsync(int id)
        {
            var entity = await _dbcontext.ProductCategories.AsNoTracking()
                .FirstOrDefaultAsync(category => category.Id == id);
            return entity is null
                ? throw new KeyNotFoundException($"Product category with ID {id} was not found.")
                : ToDto(entity);
        }

        public async Task AddProductCategoryAsync(AddProductCategoryDTO productCategory)
        {
            ArgumentNullException.ThrowIfNull(productCategory);
            await _dbcontext.ProductCategories.AddAsync(new ProductCategory
            {
                ProductCategoryName = productCategory.ProductCategoryName,
                ProductCategoryDescription = productCategory.ProductCategoryDescription,
                Status = productCategory.Status,
                UserAdded = productCategory.UserAdded,
                DataAdded = productCategory.DateAdded
            });
            await _dbcontext.SaveChangesAsync();
        }

        private static GetProductCategoryDTO ToDto(ProductCategory entity) => new()
        {
            Id = entity.Id,
            ProductCategoryName = entity.ProductCategoryName,
            ProductCategoryDescription = entity.ProductCategoryDescription,
            Status = entity.Status,
            UserAdded = entity.UserAdded,
            DateAdded = entity.DataAdded
        };
    }
}
