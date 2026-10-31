using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PaymentRepository : IPayment
    {
        private readonly ApplicationDbContext _dbcontext;

        public PaymentRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<GetPaymentDTO>> GetAllPaymentAsync()
        {
            var entities = await _dbcontext.Payments.AsNoTracking().ToListAsync();
            return entities.Select(ToDto).ToList();
        }

        public async Task<GetPaymentDTO> GetPaymentByIdAsync(int id)
        {
            var entity = await _dbcontext.Payments.AsNoTracking()
                .FirstOrDefaultAsync(payment => payment.Id == id);
            return entity is null
                ? throw new KeyNotFoundException($"Payment with ID {id} was not found.")
                : ToDto(entity);
        }

        public async Task AddPaymentAsync(AddPaymentDTO payment)
        {
            ArgumentNullException.ThrowIfNull(payment);
            await _dbcontext.Payments.AddAsync(new Payment
            {
                PaymentName = payment.PaymentName,
                PaymentAmount = RepositoryValueConversions.ToStorageString(payment.PaymentAmount),
                PaymentDate = RepositoryValueConversions.ToStorageString(payment.PaymentDate),
                PaymentDescription = payment.PaymentDescription,
                PaymentMethodId = payment.PaymentMethodId,
                SalesId = payment.SalesId,
                AccountId = payment.AccountId,
                DateAdded = payment.DateAdded,
                UserAdded = payment.UserAdded,
                Status = payment.Status
            });
            await _dbcontext.SaveChangesAsync();
        }

        private static GetPaymentDTO ToDto(Payment entity) => new()
        {
            Id = entity.Id,
            PaymentName = entity.PaymentName,
            PaymentAmount = RepositoryValueConversions.ToDecimal(entity.PaymentAmount),
            PaymentDate = RepositoryValueConversions.ToDateTime(entity.PaymentDate),
            PaymentDescription = entity.PaymentDescription,
            DateAdded = entity.DateAdded,
            UserAdded = entity.UserAdded,
            Status = entity.Status
        };
    }
}
