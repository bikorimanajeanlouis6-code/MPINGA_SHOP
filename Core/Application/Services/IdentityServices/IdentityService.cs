using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
namespace Application.Services.IdentityServices
{
    public class IdentityService:IIdentityService
    {
        private readonly IIdentity _identity;
        public IdentityService(IIdentity identity)
        {
            _identity=identity;
        }
        public async Task<List<GetUserDTO>> GetAllUsersAsync()
        {
           return await _identity.GetAllUsersAsync();
        }
        public async Task AddUserAsync(AddUserDTO user)
        {
            await _identity.AddUserAsync(user);
        }

         public async Task<bool> LoginAsync(LoginDTO login)
        {
            return await _identity.LoginAsync(login);
        }
          public async Task LogOutAsync()
        {
            await _identity.LogoutAsync();
        }
    }
   
}
