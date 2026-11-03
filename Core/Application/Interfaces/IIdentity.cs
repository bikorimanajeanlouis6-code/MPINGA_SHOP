using Application.DTOs;
namespace Application.Interfaces
{
      public interface IIdentity
    {
        Task AddUserAsync(AddUserDTO register);
        Task<List<GetUserDTO>> GetAllUsersAsync();

        Task<bool>LoginAsync(LoginDTO login);
        Task LogoutAsync();

    }
}
