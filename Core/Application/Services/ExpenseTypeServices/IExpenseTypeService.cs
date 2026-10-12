using Application.DTOs;

namespace Application.Services.ExpenseTypeServices
{
    public interface IExpenseTypeService
    {
           Task <List<GetExpenseTypeDTO>>GetAllExpenseTypeAsync();
           Task AddExpenseTypeAsync(AddExpenseTypeDTO expenseType); 
           Task <GetExpenseTypeDTO> GetExpenseTypeByIdAsync(int id);
       
    }
}