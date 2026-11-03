using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Repositories
{
    public class ExpenseRepository : IExpense
    {
        private readonly ApplicationDbContext _dbcontext;

        public ExpenseRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<GetExpenseDTO>> GetAllExpenseAsync()
        {
            var entities = await _dbcontext.Expenses
                .AsNoTracking()
                .Include(expense => expense.ExpenseType)
                .Include(expense => expense.Account)
                .OrderByDescending(expense => expense.Id)
                .ToListAsync();
            return entities.Select(ToDto).ToList();
        }

        public async Task<GetExpenseDTO> GetExpenseByIdAsync(int id)
        {
            var entity = await _dbcontext.Expenses.AsNoTracking()
                .Include(expense => expense.ExpenseType)
                .Include(expense => expense.Account)
                .FirstOrDefaultAsync(expense => expense.Id == id);
            return entity is null
                ? throw new KeyNotFoundException($"Expense with ID {id} was not found.")
                : ToDto(entity);
        }

        public async Task AddExpenseAsync(AddExpenseDTO expense)
        {
            ArgumentNullException.ThrowIfNull(expense);
            if (expense.ExpenseAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(expense), "Expense amount must be greater than zero.");
            if (expense.AccountId is not > 0)
                throw new ArgumentOutOfRangeException(nameof(expense), "An account is required for every expense.");

            await using var transaction = await _dbcontext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            var account = await _dbcontext.Accounts
                .FirstOrDefaultAsync(existingAccount => existingAccount.Id == expense.AccountId.Value)
                ?? throw new KeyNotFoundException($"Account with ID {expense.AccountId.Value} was not found.");
            account.Balance -= expense.ExpenseAmount;

            await _dbcontext.Expenses.AddAsync(new Expenses
            {
                ExpenseName = expense.ExpenseName,
                ExpenseAmount = RepositoryValueConversions.ToStorageString(expense.ExpenseAmount),
                ExpenseDate = expense.ExpenseDate,
                ExpenseTypeId = expense.ExpenseTypeId,
                AccountId = expense.AccountId,
                DateAdded = expense.DateAdded,
                UserAdded = expense.UserAdded,
                Status = expense.Status
            });
            await _dbcontext.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        private static GetExpenseDTO ToDto(Expenses entity) => new()
        {
            Id = entity.Id,
            ExpenseName = entity.ExpenseName,
            ExpenseAmount = RepositoryValueConversions.ToDecimal(entity.ExpenseAmount),
            ExpenseDate = entity.ExpenseDate,
            ExpenseTypeName = entity.ExpenseType?.ExpenseTypeName ?? string.Empty,
            AccountName = entity.Account?.AccountName ?? string.Empty,
            AccountId = entity.AccountId,
            DateAdded = entity.DateAdded,
            UserAdded = entity.UserAdded,
            Status = entity.Status
        };
    }
}
