using Application.DTOs;

namespace Application.Services.ExpenseServices
{
    public interface IExpenseService
    {
           Task <List<GetExpenseDTO>>GetAllExpenseAsync();
           Task AddExpenseAsync(AddExpenseDTO expense); 
           Task <GetExpenseDTO> GetExpenseByIdAsync(int id);
       
    }
}