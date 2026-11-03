using Application.DTOs;

namespace Application.Services.AccountServices
{
    public interface IAccountService
    {
           Task <List<GetAccountDTO>>GetAllAccountAsync();
           Task AddAccountAsync(AddAccountDTO account); 
           Task <GetAccountDTO> GetAccountByIdAsync(int id);
           Task UpdateAccountBalanceAsync(int id, decimal balance);
           Task DepositIntoAccountAsync(int id, decimal amount, string description);
           Task<List<GetAccountDepositDTO>> GetAccountDepositsAsync(int accountId);
       
    }
}