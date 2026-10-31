using Application.DTOs;
using Application.Interfaces;

namespace Application.Services.ExpenseTypeServices
{
    public class ExpenseTypeService : IExpenseTypeService
    {
        private readonly IExpenseType _expenseTypeRepository;

        public ExpenseTypeService(IExpenseType expenseTypeRepository)
        {
            _expenseTypeRepository = expenseTypeRepository;
        }

        public Task<List<GetExpenseTypeDTO>> GetAllExpenseTypeAsync() =>
            _expenseTypeRepository.GetAllExpenseTypeAsync();

        public Task<GetExpenseTypeDTO> GetExpenseTypeByIdAsync(int id) =>
            _expenseTypeRepository.GetExpenseTypeByIdAsync(id);

        public Task AddExpenseTypeAsync(AddExpenseTypeDTO expenseType) =>
            _expenseTypeRepository.AddExpenseTypeAsync(expenseType);
    }
}