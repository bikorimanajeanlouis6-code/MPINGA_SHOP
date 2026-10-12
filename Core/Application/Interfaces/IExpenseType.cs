using Application.DTOs;

namespace Application.Interfaces;

public interface IExpenseType
{
    Task<List<GetExpenseTypeDTO>> GetAllExpenseTypeAsync();
    Task<GetExpenseTypeDTO> GetExpenseTypeByIdAsync(int id);
    Task AddExpenseTypeAsync(AddExpenseTypeDTO expenseType);
}