using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ExpenseTypeRepository : IExpenseType
    {
        private readonly ApplicationDbContext _dbcontext;

        public ExpenseTypeRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<GetExpenseTypeDTO>> GetAllExpenseTypeAsync()
        {
            var entities = await _dbcontext.ExpenseTypes.AsNoTracking().ToListAsync();
            return entities.Select(ToDto).ToList();
        }

        public async Task<GetExpenseTypeDTO> GetExpenseTypeByIdAsync(int id)
        {
            var entity = await _dbcontext.ExpenseTypes.AsNoTracking()
                .FirstOrDefaultAsync(expenseType => expenseType.Id == id);
            return entity is null
                ? throw new KeyNotFoundException($"Expense type with ID {id} was not found.")
                : ToDto(entity);
        }

        public async Task AddExpenseTypeAsync(AddExpenseTypeDTO expenseType)
        {
            ArgumentNullException.ThrowIfNull(expenseType);
            await _dbcontext.ExpenseTypes.AddAsync(new ExpenseType
            {
                ExpenseTypeName = expenseType.ExpenseTypeName,
                ExpenseTypeDescription = expenseType.ExpenseTypeDescription,
                Status = expenseType.Status,
                UserAdded = expenseType.UserAdded,
                DateAdded = expenseType.DateAdded
            });
            await _dbcontext.SaveChangesAsync();
        }

        private static GetExpenseTypeDTO ToDto(ExpenseType entity) => new()
        {
            Id = entity.Id,
            ExpenseTypeName = entity.ExpenseTypeName,
            ExpenseTypeDescription = entity.ExpenseTypeDescription,
            Status = entity.Status,
            UserAdded = entity.UserAdded,
            DateAdded = entity.DateAdded
        };
    }
}
