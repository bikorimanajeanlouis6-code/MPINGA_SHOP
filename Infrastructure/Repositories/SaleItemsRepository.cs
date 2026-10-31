using System.Globalization;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SaleItemsRepository : ISaleItems
    {
        private readonly ApplicationDbContext _dbcontext;

        public SaleItemsRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<GetSaleItemsDTO>> GetAllSaleItemsAsync()
        {
            var entities = await _dbcontext.SalesItems
                .AsNoTracking()
                .Include(item => item.Sales)
                    .ThenInclude(sale => sale!.Customer)
                .Include(item => item.Product)
                .ToListAsync();
            return entities.Select(ToDto).ToList();
        }

        public async Task<GetSaleItemsDTO> GetSaleItemsByIdAsync(int id)
        {
            var entity = await _dbcontext.SalesItems
                .AsNoTracking()
                .Include(item => item.Sales)
                    .ThenInclude(sale => sale!.Customer)
                .Include(item => item.Product)
                .FirstOrDefaultAsync(item => item.Id == id);
            return entity is null
                ? throw new KeyNotFoundException($"Sale item with ID {id} was not found.")
                : ToDto(entity);
        }

        public Task AddSaleItemsAsync(AddSaleItemsDTO saleItem) =>
            AddSaleItemsAsync(new List<AddSaleItemsDTO> { saleItem });

        public async Task AddSaleItemsAsync(List<AddSaleItemsDTO> saleItems)
        {
            ArgumentNullException.ThrowIfNull(saleItems);
            if (saleItems.Count == 0)
                throw new ArgumentException("At least one sale item is required.", nameof(saleItems));

            var salesId = saleItems[0].SalesId;
            if (salesId <= 0 || saleItems.Any(item => item.SalesId != salesId))
                throw new ArgumentException("All sale items must belong to the same valid sale.", nameof(saleItems));

            var validatedItems = saleItems.Select(item =>
            {
                if (item.ProductId <= 0
                    || !decimal.TryParse(item.SaleItemPrice, NumberStyles.Number, CultureInfo.InvariantCulture, out var price)
                    || price <= 0
                    || !int.TryParse(item.SaleItemQuantity, NumberStyles.Integer, CultureInfo.InvariantCulture, out var quantity)
                    || quantity <= 0)
                {
                    throw new ArgumentException("Each item must have a product, a positive price, and a positive whole-number quantity.", nameof(saleItems));
                }

                return (Item: item, Price: price, Quantity: quantity);
            }).ToList();

            await using var transaction = await _dbcontext.Database.BeginTransactionAsync();

            var sale = await _dbcontext.Sales
                .FirstOrDefaultAsync(existingSale => existingSale.Id == salesId)
                ?? throw new KeyNotFoundException($"Sale with ID {salesId} was not found.");

            var productIds = validatedItems.Select(item => item.Item.ProductId).Distinct().ToList();
            var products = await _dbcontext.Products
                .Include(product => product.ProductCategory)
                .Where(product => productIds.Contains(product.Id))
                .ToDictionaryAsync(product => product.Id);

            if (products.Count != productIds.Count)
                throw new KeyNotFoundException("One or more selected products were not found.");

            var addedTotal = 0m;
            var now = DateTime.Now;
            var itemEntities = validatedItems.Select(validated =>
            {
                var product = products[validated.Item.ProductId];
                var total = validated.Price * validated.Quantity;
                addedTotal += total;

                return new SaleItems
                {
                    SaleItemName = product.ProductName,
                    SaleItemPrice = RepositoryValueConversions.ToStorageString(validated.Price),
                    SaleItemQuantity = RepositoryValueConversions.ToStorageString(validated.Quantity),
                    TotalSaleItemPrice = RepositoryValueConversions.ToStorageString(total),
                    SaleItemCategory = product.ProductCategory?.ProductCategoryName ?? string.Empty,
                    SalesId = salesId,
                    ProductId = product.Id,
                    DateAdded = now,
                    UserAdded = validated.Item.UserAdded,
                    Status = "Active"
                };
            }).ToList();

            var totalConsumed = RepositoryValueConversions.ToDecimal(sale.TotalConsumed) + addedTotal;
            var totalPaid = RepositoryValueConversions.ToDecimal(sale.TotalPaid);
            sale.TotalConsumed = RepositoryValueConversions.ToStorageString(totalConsumed);
            sale.Balance = RepositoryValueConversions.ToStorageString(totalConsumed - totalPaid);

            await _dbcontext.SalesItems.AddRangeAsync(itemEntities);
            await _dbcontext.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        private static GetSaleItemsDTO ToDto(SaleItems entity) => new()
        {
            Id = entity.Id,
            SaleItemName = entity.SaleItemName,
            SaleItemPrice = entity.SaleItemPrice,
            SaleItemQuantity = entity.SaleItemQuantity,
            TotalSaleItemPrice = entity.TotalSaleItemPrice,
            CustomerName = entity.Sales?.Customer?.Name ?? string.Empty,
            SaleItemCategory = entity.SaleItemCategory,
            DateAdded = entity.DateAdded,
            UserAdded = entity.UserAdded,
            Status = entity.Status,
            SalesId = entity.SalesId,
            ProductId = entity.ProductId
        };
    }
}
