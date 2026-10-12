using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SalesRepository : ISales
    {
        private readonly ApplicationDbContext _dbcontext;

        public SalesRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<GetSalesDTO>> GetAllSalesAsync()
        {
            var entities = await _dbcontext.Sales
                .AsNoTracking()
                .Include(sale => sale.Customer)
                .ToListAsync();
            return entities.Select(ToDto).ToList();
        }

        public async Task<GetSalesDTO> GetSalesByIdAsync(int id)
        {
            var entity = await _dbcontext.Sales.AsNoTracking()
                .Include(sale => sale.Customer)
                .FirstOrDefaultAsync(sales => sales.Id == id);
            return entity is null
                ? throw new KeyNotFoundException($"Sale with ID {id} was not found.")
                : ToDto(entity);
        }

        public async Task AddSalesAsync(AddSalesDTO sales)
        {
            ArgumentNullException.ThrowIfNull(sales);
            await _dbcontext.Sales.AddAsync(new Sales
            {
                TotalConsumed = sales.TotalConsumed,
                Balance = sales.Balance,
                TotalPaid = sales.TotalPaid,
                SaleDate = sales.SaleDate,
                CustomerId = sales.CustomerId,
                UserAdded = sales.UserAdded,
                Status = sales.Status
            });
            await _dbcontext.SaveChangesAsync();
        }

        private static GetSalesDTO ToDto(Sales entity) => new()
        {
            Id = entity.Id,
            CustomerName = entity.Customer?.Name ?? string.Empty,
            TotalConsumed = entity.TotalConsumed,
            Balance = entity.Balance,
            TotalPaid = entity.TotalPaid,
            SaleDate = entity.SaleDate,
            UserAdded = entity.UserAdded,
            Status = entity.Status
        };
    }
}
