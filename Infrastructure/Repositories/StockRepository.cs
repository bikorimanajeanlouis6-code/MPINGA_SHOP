using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class StockRepository : IStock
    {
        private readonly ApplicationDbContext _dbcontext;

        public StockRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<GetStockDTO>> GetAllStockAsync()
        {
            var entities = await _dbcontext.Stocks.AsNoTracking().ToListAsync();
            return entities.Select(ToDto).ToList();
        }

        public async Task<GetStockDTO> GetStockByIdAsync(int id)
        {
            var entity = await _dbcontext.Stocks.AsNoTracking()
                .FirstOrDefaultAsync(stock => stock.Id == id);
            return entity is null
                ? throw new KeyNotFoundException($"Stock with ID {id} was not found.")
                : ToDto(entity);
        }

        public async Task AddStockAsync(AddStockDTO stock)
        {
            ArgumentNullException.ThrowIfNull(stock);
            await _dbcontext.Stocks.AddAsync(new Stock
            {
                StockName = stock.StockName,
                QuantityInStock = RepositoryValueConversions.ToStorageString(stock.QuantityInStock),
                QuantitySold = RepositoryValueConversions.ToStorageString(stock.QuantitySold),
                QuantityAvailable = RepositoryValueConversions.ToStorageString(stock.QuantityAvailable),
                Unit = stock.Unit,
                SellingPrice = RepositoryValueConversions.ToStorageString(stock.SellingPrice),
                LastPrice = RepositoryValueConversions.ToStorageString(stock.LastPrice),
                LastlyModified = RepositoryValueConversions.ToStorageString(stock.LastlyModified),
                ProductId = stock.ProductId,
                DateAdded = stock.DateAdded,
                UserAdded = stock.UserAdded,
                Status = stock.Status
            });
            await _dbcontext.SaveChangesAsync();
        }

        private static GetStockDTO ToDto(Stock entity) => new()
        {
            Id = entity.Id,
            StockName = entity.StockName,
            QuantityInStock = RepositoryValueConversions.ToInt32(entity.QuantityInStock),
            QuantitySold = RepositoryValueConversions.ToInt32(entity.QuantitySold),
            QuantityAvailable = RepositoryValueConversions.ToInt32(entity.QuantityAvailable),
            Unit = entity.Unit,
            SellingPrice = RepositoryValueConversions.ToDecimal(entity.SellingPrice),
            LastPrice = RepositoryValueConversions.ToDecimal(entity.LastPrice),
            LastlyModified = RepositoryValueConversions.ToDateTime(entity.LastlyModified),
            DateAdded = entity.DateAdded,
            UserAdded = entity.UserAdded,
            Status = entity.Status
        };
    }
}
