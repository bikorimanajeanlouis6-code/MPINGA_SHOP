using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Repositories
{
    public class ReplenishmentRepository : IReplenishment
    {
        private readonly ApplicationDbContext _dbcontext;

        public ReplenishmentRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<GetReplenishmentDTO>> GetAllReplenishmentsAsync()
        {
            var entities = await _dbcontext.Replenishments
                .AsNoTracking()
                .Include(replenishment => replenishment.Product)
                    .ThenInclude(product => product!.Stocks)
                .OrderByDescending(replenishment => replenishment.Id)
                .ToListAsync();
            return entities.Select(ToDto).ToList();
        }

        public async Task<GetReplenishmentDTO> GetReplenishmentByIdAsync(int id)
        {
            var entity = await _dbcontext.Replenishments.AsNoTracking()
                .Include(replenishment => replenishment.Product)
                    .ThenInclude(product => product!.Stocks)
                .FirstOrDefaultAsync(replenishment => replenishment.Id == id);
            return entity is null
                ? throw new KeyNotFoundException($"Replenishment with ID {id} was not found.")
                : ToDto(entity);
        }

        public async Task AddReplenishmentAsync(AddReplenishmentDTO replenishment)
        {
            ArgumentNullException.ThrowIfNull(replenishment);
            if (replenishment.ProductId <= 0
                || replenishment.QuantityReplenished <= 0
                || !decimal.TryParse(
                    replenishment.PurchasePrice,
                    System.Globalization.NumberStyles.Number,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out var purchasePrice)
                || purchasePrice <= 0)
            {
                throw new ArgumentException(
                    "A product, positive quantity, and positive purchase price are required.",
                    nameof(replenishment));
            }

            var total = purchasePrice * replenishment.QuantityReplenished;
            if (replenishment.PaidAmount < 0 || replenishment.PaidAmount > total)
                throw new ArgumentOutOfRangeException(nameof(replenishment), "The initial supplier payment must be between zero and the replenishment total.");
            if (replenishment.PaidAmount > 0
                && (replenishment.AccountId is not > 0 || replenishment.PaymentMethodId is not > 0))
            {
                throw new ArgumentException(
                    "An account and payment method are required when recording an initial supplier payment.",
                    nameof(replenishment));
            }

            var unit = replenishment.Unit.Trim().ToLowerInvariant();
            if (unit is not ("kg" or "pcs"))
                throw new ArgumentException("Replenishment unit must be kg or pcs.", nameof(replenishment));

            await using var transaction = await _dbcontext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            var product = await _dbcontext.Products
                .Include(existingProduct => existingProduct.Stocks)
                .FirstOrDefaultAsync(existingProduct => existingProduct.Id == replenishment.ProductId)
                ?? throw new KeyNotFoundException($"Product with ID {replenishment.ProductId} was not found.");

            var productUnit = NormalizeUnit(product.Unit);
            if (!string.Equals(productUnit, unit, StringComparison.Ordinal))
                throw new InvalidOperationException($"Replenishment unit must match the product unit ({productUnit}).");

            var existingUnit = product.Stocks?
                .Select(stock => NormalizeUnit(stock.Unit))
                .FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(existingUnit)
                && !string.Equals(existingUnit, unit, StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"The selected product already uses {existingUnit}. Replenishment must use the same unit.");
            }

            Account? account = null;
            if (replenishment.PaidAmount > 0)
            {
                var accountId = replenishment.AccountId.GetValueOrDefault();
                account = await _dbcontext.Accounts
                    .FirstOrDefaultAsync(existingAccount => existingAccount.Id == accountId)
                    ?? throw new KeyNotFoundException($"Account with ID {accountId} was not found.");
            }

            var replenishmentEntity = new Replenishment
            {
                ReplenishmentName = replenishment.ReplenishmentName,
                ReplenishmentAmount = RepositoryValueConversions.ToStorageString(total),
                ReplenishmentDate = RepositoryValueConversions.ToStorageString(replenishment.ReplenishmentDate),
                QuantityReplenished = RepositoryValueConversions.ToStorageString(replenishment.QuantityReplenished),
                SellingPrice = RepositoryValueConversions.ToStorageString(replenishment.SellingPrice),
                PurchasePrice = RepositoryValueConversions.ToStorageString(replenishment.PurchasePrice),
                totalpurchaseprice = RepositoryValueConversions.ToStorageString(total),
                SupplierName = replenishment.SupplierName,
                SupplierPhone = replenishment.SupplierPhone,
                ProductId = replenishment.ProductId,
                DateAdded = replenishment.DateAdded,
                UserAdded = replenishment.UserAdded,
                Status = replenishment.Status
            };
            await _dbcontext.Replenishments.AddAsync(replenishmentEntity);
            var modifiedAt = DateTime.Now;
            var stock = product.Stocks?
                .OrderByDescending(existingStock => string.Equals(existingStock.Status, "Active", StringComparison.OrdinalIgnoreCase))
                .ThenByDescending(existingStock => existingStock.DateAdded)
                .ThenByDescending(existingStock => existingStock.Id)
                .FirstOrDefault();
            var hasSellingPrice = decimal.TryParse(
                replenishment.SellingPrice,
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.InvariantCulture,
                out var sellingPrice);

            if (stock is null)
            {
                await _dbcontext.Stocks.AddAsync(new Stock
                {
                    StockName = product.ProductName,
                    QuantityInStock = RepositoryValueConversions.ToStorageString(replenishment.QuantityReplenished),
                    QuantitySold = "0",
                    QuantityAvailable = RepositoryValueConversions.ToStorageString(replenishment.QuantityReplenished),
                    Unit = unit,
                    SellingPrice = RepositoryValueConversions.ToStorageString(hasSellingPrice ? sellingPrice : 0m),
                    LastPrice = RepositoryValueConversions.ToStorageString(purchasePrice),
                    LastlyModified = RepositoryValueConversions.ToStorageString(modifiedAt),
                    ProductId = product.Id,
                    DateAdded = modifiedAt,
                    UserAdded = replenishment.UserAdded,
                    Status = "Active"
                });
            }
            else
            {
                var quantityInStock = RepositoryValueConversions.ToInt32(stock.QuantityInStock);
                var quantityAvailable = RepositoryValueConversions.ToInt32(stock.QuantityAvailable);
                stock.StockName = product.ProductName;
                stock.QuantityInStock = RepositoryValueConversions.ToStorageString(
                    checked(quantityInStock + replenishment.QuantityReplenished));
                stock.QuantityAvailable = RepositoryValueConversions.ToStorageString(
                    checked(quantityAvailable + replenishment.QuantityReplenished));
                stock.Unit = unit;
                if (hasSellingPrice)
                    stock.SellingPrice = RepositoryValueConversions.ToStorageString(sellingPrice);
                stock.LastPrice = RepositoryValueConversions.ToStorageString(purchasePrice);
                stock.LastlyModified = RepositoryValueConversions.ToStorageString(modifiedAt);
                stock.Status = "Active";
            }
            await _dbcontext.SaveChangesAsync();

            if (replenishment.PaidAmount > 0)
            {
                account!.Balance -= replenishment.PaidAmount;
                await _dbcontext.Payments.AddAsync(new Payment
                {
                    PaymentName = $"Supplier payment - {replenishment.SupplierName}",
                    PaymentAmount = RepositoryValueConversions.ToStorageString(replenishment.PaidAmount),
                    PaymentDate = RepositoryValueConversions.ToStorageString(replenishment.ReplenishmentDate),
                    PaymentDescription = $"Initial supplier payment for replenishment #{replenishmentEntity.Id}",
                    PaymentMethodId = replenishment.PaymentMethodId!.Value,
                    AccountId = account.Id,
                    ReplenishmentId = replenishmentEntity.Id,
                    DateAdded = replenishment.DateAdded,
                    UserAdded = replenishment.UserAdded,
                    Status = "Active"
                });
                await _dbcontext.SaveChangesAsync();
            }

            await transaction.CommitAsync();
        }

        private static GetReplenishmentDTO ToDto(Replenishment entity)
        {
            var quantity = int.TryParse(
                entity.QuantityReplenished,
                System.Globalization.NumberStyles.Integer,
                System.Globalization.CultureInfo.InvariantCulture,
                out var parsedQuantity)
                ? parsedQuantity
                : 0;
            var hasPurchasePrice = decimal.TryParse(
                entity.PurchasePrice,
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.InvariantCulture,
                out var purchasePrice);
            var total = hasPurchasePrice
                ? purchasePrice * quantity
                : decimal.TryParse(
                    entity.totalpurchaseprice,
                    System.Globalization.NumberStyles.Number,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out var existingTotal)
                    ? existingTotal
                    : 0m;
            var totalString = RepositoryValueConversions.ToStorageString(total);

            return new GetReplenishmentDTO
            {
                Id = entity.Id,
                ProductId = entity.ProductId,
                ReplenishmentName = entity.ReplenishmentName,
                ReplenishmentAmount = totalString,
                ReplenishmentDate = RepositoryValueConversions.ToDateTime(entity.ReplenishmentDate),
                QuantityReplenished = quantity,
                SellingPrice = entity.SellingPrice,
                PurchasePrice = entity.PurchasePrice,
                TotalPurchasePrice = totalString,
                SupplierName = entity.SupplierName,
                SupplierPhone = entity.SupplierPhone,
                Unit = entity.Product?.Stocks?
                    .OrderByDescending(stock => stock.DateAdded)
                    .Select(stock => stock.Unit)
                    .FirstOrDefault() ?? string.Empty,
                DateAdded = entity.DateAdded,
                UserAdded = entity.UserAdded,
                Status = entity.Status
            };
        }

        private static string NormalizeUnit(string? unit) =>
            unit?.Trim().ToLowerInvariant() switch
            {
                "kg" or "kgs" or "kilogram" or "kilograms" => "kg",
                "pc" or "pcs" or "piece" or "pieces" or "pieces (pcs)" => "pcs",
                _ => unit?.Trim() ?? string.Empty
            };
    }
}
