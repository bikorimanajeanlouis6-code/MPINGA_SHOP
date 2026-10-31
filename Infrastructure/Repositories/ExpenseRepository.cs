using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
                .ToListAsync();
            return entities.Select(ToDto).ToList();
        }

        public async Task<GetExpenseDTO> GetExpenseByIdAsync(int id)
        {
            var entity = await _dbcontext.Expenses.AsNoTracking()
                .Include(expense => expense.ExpenseType)
                .FirstOrDefaultAsync(expense => expense.Id == id);
            return entity is null
                ? throw new KeyNotFoundException($"Expense with ID {id} was not found.")
                : ToDto(entity);
        }

        public async Task AddExpenseAsync(AddExpenseDTO expense)
        {
            ArgumentNullException.ThrowIfNull(expense);
            await _dbcontext.Expenses.AddAsync(new Expenses
            {
                ExpenseName = expense.ExpenseName,
                ExpenseAmount = RepositoryValueConversions.ToStorageString(expense.ExpenseAmount),
                ExpenseDate = expense.ExpenseDate,
                ExpenseTypeId = expense.ExpenseTypeId,
                DateAdded = expense.DateAdded,
                UserAdded = expense.UserAdded,
                Status = expense.Status
            });
            await _dbcontext.SaveChangesAsync();
        }

        private static GetExpenseDTO ToDto(Expenses entity) => new()
        {
            Id = entity.Id,
            ExpenseName = entity.ExpenseName,
            ExpenseAmount = RepositoryValueConversions.ToDecimal(entity.ExpenseAmount),
            ExpenseDate = entity.ExpenseDate,
            ExpenseTypeName = entity.ExpenseType?.ExpenseTypeName ?? string.Empty,
            DateAdded = entity.DateAdded,
            UserAdded = entity.UserAdded,
            Status = entity.Status
        };
    }
}
