using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PaymentMethordRepository : IPaymentMethord
    {
        private readonly ApplicationDbContext _dbcontext;

        public PaymentMethordRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<GetPaymentMethordDTO>> GetAllPaymentMethordAsync()
        {
            var entities = await _dbcontext.PaymentMethods.AsNoTracking().ToListAsync();
            return entities.Select(ToDto).ToList();
        }

        public async Task<GetPaymentMethordDTO> GetPaymentMethordByIdAsync(int id)
        {
            var entity = await _dbcontext.PaymentMethods.AsNoTracking()
                .FirstOrDefaultAsync(paymentMethod => paymentMethod.Id == id);
            return entity is null
                ? throw new KeyNotFoundException($"Payment method with ID {id} was not found.")
                : ToDto(entity);
        }

        public async Task AddPaymentMethordAsync(AddPaymentMethordDTO paymentMethord)
        {
            ArgumentNullException.ThrowIfNull(paymentMethord);
            await _dbcontext.PaymentMethods.AddAsync(new PaymentMethod
            {
                PaymentMethodName = paymentMethord.PaymentMethodName,
                Status = paymentMethord.Status,
                UserAdded = paymentMethord.UserAdded,
                DataAdded = paymentMethord.DateAdded
            });
            await _dbcontext.SaveChangesAsync();
        }

        private static GetPaymentMethordDTO ToDto(PaymentMethod entity) => new()
        {
            Id = entity.Id,
            PaymentMethodName = entity.PaymentMethodName,
            Status = entity.Status,
            UserAdded = entity.UserAdded,
            DateAdded = entity.DataAdded
        };
    }
}
