using Application.DTOs;

namespace Application.Interfaces;

public interface IAccount
{
    Task<List<GetAccountDTO>> GetAllAccountAsync();
    Task<GetAccountDTO> GetAccountByIdAsync(int id);
    Task AddAccountAsync(AddAccountDTO account);
}