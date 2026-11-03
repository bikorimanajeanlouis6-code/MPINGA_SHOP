using Application.DTOs;
using Application.Interfaces;

namespace Application.Services.AccountServices
{
    public class AccountService : IAccountService
    {
        private readonly IAccount _accountRepository;

        public AccountService(IAccount accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<List<GetAccountDTO>> GetAllAccountAsync()
        {
            return _accountRepository.GetAllAccountAsync();
        }

        public Task<GetAccountDTO> GetAccountByIdAsync(int id)
        {
            return _accountRepository.GetAccountByIdAsync(id);
        }

        public Task AddAccountAsync(AddAccountDTO account)
        {
            return _accountRepository.AddAccountAsync(account);
        }

        public Task UpdateAccountBalanceAsync(int id, decimal balance)
        {
            return _accountRepository.UpdateAccountBalanceAsync(id, balance);
        }

        public Task DepositIntoAccountAsync(int id, decimal amount, string description)
        {
            return _accountRepository.DepositIntoAccountAsync(id, amount, description);
        }

        public Task<List<GetAccountDepositDTO>> GetAccountDepositsAsync(int accountId)
        {
            return _accountRepository.GetAccountDepositsAsync(accountId);
        }
    }
}