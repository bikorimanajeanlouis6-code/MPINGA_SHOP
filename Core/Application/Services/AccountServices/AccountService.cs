using Application.DTOs;

namespace Application.Services.AccountServices
{
    public class AccountService : IAccountService
    {
        public Task<List<GetAccountDTO>> GetAllAccountAsync()
        {
            return Task.FromResult(new List<GetAccountDTO>());
        }

        public Task<GetAccountDTO> GetAccountByIdAsync(int id)
        {
            return Task.FromResult(new GetAccountDTO());
        }

        public Task AddAccountAsync(AddAccountDTO account)
        {
            return Task.CompletedTask;
        }
    }
}