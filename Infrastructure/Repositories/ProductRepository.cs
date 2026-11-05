using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
            return await _dbcontext.Products
                .AsNoTracking()
                .OrderByDescending(product => product.Id)
                .Select(product => new GetProductDTO
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    ProductPrice = product.ProductPrice,
                    ProductQuantity = product.ProductQuantity,
                    Unit = product.Unit,
                    HasStock = product.Stocks.Any(),
                    ProductCategoryId = product.ProductCategoryId,
                    ProductCategoryName = product.ProductCategory.ProductCategoryName ?? string.Empty,
                    Status = product.Status,
                    UserAdded = product.UserAdded,
                    DateAdded = product.DateAdded
                })
                .ToListAsync();
        }

        public async Task<GetProductDTO> GetProductByIdAsync(int id)
        {
            var entity = await _dbcontext.Products.AsNoTracking()
                .Include(product => product.ProductCategory)
                .Include(product => product.Stocks)
                .FirstOrDefaultAsync(product => product.Id == id);
            return entity is null
                ? throw new KeyNotFoundException($"Product with ID {id} was not found.")
                : ToDto(entity);
        }

        public async Task AddProductAsync(AddProductDTO product)
        {
            ArgumentNullException.ThrowIfNull(product);
            var normalizedName = product.ProductName?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(normalizedName))
                throw new ArgumentException("Product name is required.", nameof(product));
            if (!decimal.TryParse(product.ProductQuantity, NumberStyles.Number, CultureInfo.InvariantCulture, out var quantity)
                || quantity <= 0)
                throw new ArgumentException("Product quantity must be greater than zero.", nameof(product));

            var unit = NormalizeUnit(product.Unit);
            if (unit is not ("kg" or "pcs"))
                throw new ArgumentException("Select kg or pcs as the product unit.", nameof(product));

            await EnsureNoDuplicateAsync(normalizedName, quantity);

            await _dbcontext.Products.AddAsync(new Product
            {
                ProductName = normalizedName,
                ProductDescription = product.ProductDescription,
                ProductPrice = RepositoryValueConversions.ToStorageString(product.ProductPrice),
                ProductQuantity = RepositoryValueConversions.ToStorageString(quantity),
                Unit = unit,
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

            var normalizedName = product.ProductName?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(normalizedName))
                throw new ArgumentException("Product name is required.", nameof(product));
            if (!decimal.TryParse(product.ProductQuantity, NumberStyles.Number, CultureInfo.InvariantCulture, out var quantity)
                || quantity <= 0)
                throw new ArgumentException("Product quantity must be greater than zero.", nameof(product));

            var unit = NormalizeUnit(product.Unit);
            if (unit is not ("kg" or "pcs"))
                throw new ArgumentException("Select kg or pcs as the product unit.", nameof(product));

            var entity = await _dbcontext.Products
                .FirstOrDefaultAsync(existingProduct => existingProduct.Id == product.Id)
                ?? throw new KeyNotFoundException($"Product with ID {product.Id} was not found.");

            await EnsureNoDuplicateAsync(normalizedName, quantity, entity.Id);

            var hasStock = await _dbcontext.Stocks.AnyAsync(stock => stock.ProductId == entity.Id);
            if (hasStock && !string.Equals(NormalizeUnit(entity.Unit), unit, StringComparison.Ordinal))
                throw new InvalidOperationException("The product unit cannot be changed after stock has been recorded.");

            entity.ProductName = normalizedName;
            entity.ProductDescription = product.ProductDescription;
            entity.ProductPrice = RepositoryValueConversions.ToStorageString(product.ProductPrice);
            entity.ProductQuantity = RepositoryValueConversions.ToStorageString(quantity);
            entity.Unit = unit;
            entity.ProductCategoryId = product.ProductCategoryId;
            entity.Status = product.Status;

            await _dbcontext.SaveChangesAsync();
        }

        private async Task EnsureNoDuplicateAsync(string productName, decimal quantity, int? excludedProductId = null)
        {
            var candidates = await _dbcontext.Products
                .Where(existingProduct => existingProduct.ProductName.Trim().ToLower() == productName.ToLower())
                .Select(existingProduct => new { existingProduct.Id, existingProduct.ProductQuantity })
                .ToListAsync();

            var duplicate = candidates.Any(existingProduct =>
                existingProduct.Id != excludedProductId
                && decimal.TryParse(
                    existingProduct.ProductQuantity,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out var existingQuantity)
                && existingQuantity == quantity);

            if (duplicate)
                throw new InvalidOperationException("A product with this name and quantity is already registered.");
        }

        private static string NormalizeUnit(string? unit) => unit?.Trim().ToLowerInvariant() switch
        {
            "kg" or "kgs" or "kilogram" or "kilograms" => "kg",
            "pc" or "pcs" or "piece" or "pieces" or "pieces (pcs)" => "pcs",
            _ => unit?.Trim().ToLowerInvariant() ?? string.Empty
        };

        private static GetProductDTO ToDto(Product entity) => new()
        {
            Id = entity.Id,
            ProductName = entity.ProductName,
            ProductDescription = entity.ProductDescription,
            ProductPrice = entity.ProductPrice,
            ProductQuantity = entity.ProductQuantity,
            Unit = entity.Unit,
            HasStock = entity.Stocks.Any(),
            ProductCategoryId = entity.ProductCategoryId,
            ProductCategoryName = entity.ProductCategory?.ProductCategoryName ?? string.Empty,
            Status = entity.Status,
            UserAdded = entity.UserAdded,
            DateAdded = entity.DateAdded
        };
    }
}
