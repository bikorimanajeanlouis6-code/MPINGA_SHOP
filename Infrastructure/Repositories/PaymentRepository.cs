using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
            var entities = await _dbcontext.Payments
                .AsNoTracking()
                .Include(payment => payment.PaymentMethod)
                .Include(payment => payment.Sales)
                    .ThenInclude(sale => sale!.Customer)
                .Include(payment => payment.Account)
                .OrderByDescending(payment => payment.Id)
                .ToListAsync();
            return entities.Select(ToDto).ToList();
        }

        public async Task<GetPaymentDTO> GetPaymentByIdAsync(int id)
        {
            var entity = await _dbcontext.Payments.AsNoTracking()
                .Include(payment => payment.PaymentMethod)
                .Include(payment => payment.Sales)
                    .ThenInclude(sale => sale!.Customer)
                .Include(payment => payment.Account)
                .FirstOrDefaultAsync(payment => payment.Id == id);
            return entity is null
                ? throw new KeyNotFoundException($"Payment with ID {id} was not found.")
                : ToDto(entity);
        }

        public async Task AddPaymentAsync(AddPaymentDTO payment)
        {
            ArgumentNullException.ThrowIfNull(payment);
            if (payment.PaymentAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(payment), "Payment amount must be greater than zero.");
            if (payment.PaymentMethodId <= 0)
                throw new ArgumentOutOfRangeException(nameof(payment), "A payment method is required.");
            if (payment.AccountId is not > 0)
                throw new ArgumentOutOfRangeException(nameof(payment), "An account is required for every payment.");
            if ((payment.SalesId is null) == (payment.ReplenishmentId is null))
                throw new ArgumentException("A payment must be linked to exactly one sale or replenishment.", nameof(payment));

            await using var transaction = await _dbcontext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            var account = await _dbcontext.Accounts
                .FirstOrDefaultAsync(existingAccount => existingAccount.Id == payment.AccountId.Value)
                ?? throw new KeyNotFoundException($"Account with ID {payment.AccountId.Value} was not found.");

            if (payment.SalesId is int saleId)
            {
                var sale = await _dbcontext.Sales
                    .FirstOrDefaultAsync(existingSale => existingSale.Id == saleId)
                    ?? throw new KeyNotFoundException($"Sale with ID {saleId} was not found.");
                var linkedPayments = await _dbcontext.Payments
                    .Where(existingPayment => existingPayment.SalesId == saleId
                        && existingPayment.Status == "Active")
                    .Select(existingPayment => existingPayment.PaymentAmount)
                    .ToListAsync();
                var paid = Math.Max(
                    RepositoryValueConversions.ToDecimal(sale.TotalPaid),
                    linkedPayments.Sum(RepositoryValueConversions.ToDecimal));
                var totalPaid = paid + payment.PaymentAmount;
                var totalConsumed = RepositoryValueConversions.ToDecimal(sale.TotalConsumed);
                if (totalPaid > totalConsumed)
                {
                    throw new InvalidOperationException(
                        $"Customer payment cannot exceed the sale amount consumed. Remaining amount: {Math.Max(0m, totalConsumed - paid):N2} RWF.");
                }

                sale.TotalPaid = RepositoryValueConversions.ToStorageString(totalPaid);
                sale.Balance = RepositoryValueConversions.ToStorageString(
                    totalConsumed - totalPaid);
                account.Balance += payment.PaymentAmount;
            }
            else if (payment.ReplenishmentId is int replenishmentId)
            {
                var replenishment = await _dbcontext.Replenishments
                    .FirstOrDefaultAsync(existingReplenishment => existingReplenishment.Id == replenishmentId)
                    ?? throw new KeyNotFoundException($"Replenishment with ID {replenishmentId} was not found.");
                var total = RepositoryValueConversions.ToDecimal(replenishment.ReplenishmentAmount);
                var linkedPayments = await _dbcontext.Payments
                    .Where(existingPayment => existingPayment.ReplenishmentId == replenishmentId
                        && existingPayment.Status == "Active")
                    .Select(existingPayment => existingPayment.PaymentAmount)
                    .ToListAsync();
                var paid = linkedPayments.Sum(RepositoryValueConversions.ToDecimal);
                if (paid + payment.PaymentAmount > total)
                    throw new InvalidOperationException("Supplier payment cannot exceed the replenishment's outstanding balance.");

                account.Balance -= payment.PaymentAmount;
            }

            await _dbcontext.Payments.AddAsync(new Payment
            {
                PaymentName = payment.PaymentName,
                PaymentAmount = RepositoryValueConversions.ToStorageString(payment.PaymentAmount),
                PaymentDate = RepositoryValueConversions.ToStorageString(payment.PaymentDate),
                PaymentDescription = payment.PaymentDescription,
                PaymentMethodId = payment.PaymentMethodId,
                SalesId = payment.SalesId,
                AccountId = payment.AccountId,
                ReplenishmentId = payment.ReplenishmentId,
                DateAdded = payment.DateAdded,
                UserAdded = payment.UserAdded,
                Status = payment.Status
            });
            await _dbcontext.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        private static GetPaymentDTO ToDto(Payment entity) => new()
        {
            Id = entity.Id,
            PaymentName = entity.PaymentName,
            PaymentAmount = RepositoryValueConversions.ToDecimal(entity.PaymentAmount),
            PaymentDate = RepositoryValueConversions.ToDateTime(entity.PaymentDate),
            PaymentDescription = entity.PaymentDescription,
            PaymentMethodId = entity.PaymentMethodId,
            PaymentMethodName = entity.PaymentMethod?.PaymentMethodName ?? string.Empty,
            AccountName = entity.Account?.AccountName ?? string.Empty,
            CustomerName = entity.Sales?.Customer?.Name ?? string.Empty,
            DateAdded = entity.DateAdded,
            UserAdded = entity.UserAdded,
            Status = entity.Status,
            ReplenishmentId = entity.ReplenishmentId,
            SalesId = entity.SalesId,
            AccountId = entity.AccountId
        };
    }
}
