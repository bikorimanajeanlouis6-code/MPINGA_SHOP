using Application.DTOs;

namespace Application.Services.AccountServices
{
    public interface IAccountService
    {
           Task <List<GetAccountDTO>>GetAllAccountAsync();
           Task AddAccountAsync(AddAccountDTO account); 
           Task <GetAccountDTO> GetAccountByIdAsync(int id);
       
    }
}