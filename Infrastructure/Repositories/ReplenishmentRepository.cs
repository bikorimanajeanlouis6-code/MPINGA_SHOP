using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
            var entities = await _dbcontext.Replenishments.AsNoTracking().ToListAsync();
            return entities.Select(ToDto).ToList();
        }

        public async Task<GetReplenishmentDTO> GetReplenishmentByIdAsync(int id)
        {
            var entity = await _dbcontext.Replenishments.AsNoTracking()
                .FirstOrDefaultAsync(replenishment => replenishment.Id == id);
            return entity is null
                ? throw new KeyNotFoundException($"Replenishment with ID {id} was not found.")
                : ToDto(entity);
        }

        public async Task AddReplenishmentAsync(AddReplenishmentDTO replenishment)
        {
            ArgumentNullException.ThrowIfNull(replenishment);
            await _dbcontext.Replenishments.AddAsync(new Replenishment
            {
                ReplenishmentName = replenishment.ReplenishmentName,
                ReplenishmentAmount = RepositoryValueConversions.ToStorageString(replenishment.ReplenishmentAmount),
                ReplenishmentDate = RepositoryValueConversions.ToStorageString(replenishment.ReplenishmentDate),
                QuantityReplenished = RepositoryValueConversions.ToStorageString(replenishment.QuantityReplenished),
                SellingPrice = RepositoryValueConversions.ToStorageString(replenishment.SellingPrice),
                PurchasePrice = RepositoryValueConversions.ToStorageString(replenishment.PurchasePrice),
                totalpurchaseprice = RepositoryValueConversions.ToStorageString(replenishment.TotalPurchasePrice),
                SupplierName = replenishment.SupplierName,
                SupplierPhone = replenishment.SupplierPhone,
                ProductId = replenishment.ProductId,
                DateAdded = replenishment.DateAdded,
                UserAdded = replenishment.UserAdded,
                Status = replenishment.Status
            });
            await _dbcontext.SaveChangesAsync();
        }

        private static GetReplenishmentDTO ToDto(Replenishment entity) => new()
        {
            Id = entity.Id,
            ReplenishmentName = entity.ReplenishmentName,
            ReplenishmentAmount = entity.ReplenishmentAmount,
            ReplenishmentDate = RepositoryValueConversions.ToDateTime(entity.ReplenishmentDate),
            QuantityReplenished = RepositoryValueConversions.ToInt32(entity.QuantityReplenished),
            SellingPrice = entity.SellingPrice,
            PurchasePrice = entity.PurchasePrice,
            TotalPurchasePrice = entity.totalpurchaseprice,
            SupplierName = entity.SupplierName,
            SupplierPhone = entity.SupplierPhone,
            DateAdded = entity.DateAdded,
            UserAdded = entity.UserAdded,
            Status = entity.Status
        };
    }
}
