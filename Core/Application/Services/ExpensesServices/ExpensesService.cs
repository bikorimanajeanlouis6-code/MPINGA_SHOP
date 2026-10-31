using Application.DTOs;
using Application.Interfaces;

namespace Application.Services.ExpenseServices
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpense _expenseRepository;

        public ExpenseService(IExpense expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public Task<List<GetExpenseDTO>> GetAllExpenseAsync() =>
            _expenseRepository.GetAllExpenseAsync();

        public Task<GetExpenseDTO> GetExpenseByIdAsync(int id) =>
            _expenseRepository.GetExpenseByIdAsync(id);

        public Task AddExpenseAsync(AddExpenseDTO expense) =>
            _expenseRepository.AddExpenseAsync(expense);
    }
}