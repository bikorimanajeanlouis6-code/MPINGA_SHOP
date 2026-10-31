using Application.DTOs;

namespace Application.Interfaces;

public interface IExpense
{
    Task<List<GetExpenseDTO>> GetAllExpenseAsync();
    Task<GetExpenseDTO> GetExpenseByIdAsync(int id);
    Task AddExpenseAsync(AddExpenseDTO expense);
}