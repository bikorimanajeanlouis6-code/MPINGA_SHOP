using Application.DTOs;

namespace Application.Interfaces;

public interface IAccount
{
    Task<List<GetAccountDTO>> GetAllAccountAsync();
    Task<GetAccountDTO> GetAccountByIdAsync(int id);
    Task AddAccountAsync(AddAccountDTO account);
    Task UpdateAccountBalanceAsync(int id, decimal balance);
    Task DepositIntoAccountAsync(int id, decimal amount, string description);
    Task<List<GetAccountDepositDTO>> GetAccountDepositsAsync(int accountId);
}