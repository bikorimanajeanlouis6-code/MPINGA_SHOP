using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProduct
    {
        private readonly ApplicationDbContext _dbcontext;

        public ProductRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<GetProductDTO>> GetAllProductAsync()
        {
            var entities = await _dbcontext.Products
                .AsNoTracking()
                .Include(product => product.ProductCategory)
                .OrderByDescending(product => product.Id)
                .ToListAsync();
            return entities.Select(ToDto).ToList();
        }

        public async Task<GetProductDTO> GetProductByIdAsync(int id)
        {
            var entity = await _dbcontext.Products.AsNoTracking()
                .Include(product => product.ProductCategory)
                .FirstOrDefaultAsync(product => product.Id == id);
            return entity is null
                ? throw new KeyNotFoundException($"Product with ID {id} was not found.")
                : ToDto(entity);
        }

        public async Task AddProductAsync(AddProductDTO product)
        {
            ArgumentNullException.ThrowIfNull(product);

            await _dbcontext.Products.AddAsync(new Product
            {
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductPrice = RepositoryValueConversions.ToStorageString(product.ProductPrice),
                ProductQuantity = RepositoryValueConversions.ToStorageString(product.ProductQuantity),
                ProductCategoryId = product.ProductCategoryId,
                Status = product.Status,
                UserAdded = product.UserAdded,
                DateAdded = product.DateAdded
            });
            await _dbcontext.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(AddProductDTO product)
        {
            ArgumentNullException.ThrowIfNull(product);
            if (product.Id <= 0)
                throw new ArgumentException("A valid product ID is required.", nameof(product));

            var entity = await _dbcontext.Products
                .FirstOrDefaultAsync(existingProduct => existingProduct.Id == product.Id)
                ?? throw new KeyNotFoundException($"Product with ID {product.Id} was not found.");

            entity.ProductName = product.ProductName;
            entity.ProductDescription = product.ProductDescription;
            entity.ProductPrice = RepositoryValueConversions.ToStorageString(product.ProductPrice);
            entity.ProductQuantity = RepositoryValueConversions.ToStorageString(product.ProductQuantity);
            entity.ProductCategoryId = product.ProductCategoryId;
            entity.Status = product.Status;

            await _dbcontext.SaveChangesAsync();
        }

        private static GetProductDTO ToDto(Product entity) => new()
        {
            Id = entity.Id,
            ProductName = entity.ProductName,
            ProductDescription = entity.ProductDescription,
            ProductPrice = entity.ProductPrice,
            ProductQuantity = entity.ProductQuantity,
            ProductCategoryId = entity.ProductCategoryId,
            ProductCategoryName = entity.ProductCategory?.ProductCategoryName ?? string.Empty,
            Status = entity.Status,
            UserAdded = entity.UserAdded,
            DateAdded = entity.DateAdded
        };
    }
}
