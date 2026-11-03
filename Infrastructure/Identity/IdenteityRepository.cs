using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.ValueObjects;
namespace Infrastructure.Identity
{
    
    public class IdentityRepository:IIdentity
    {
        private readonly ApplicationDbContext _dbcontext;
        public readonly SignInManager<User> _signinManager;
        public readonly UserManager<User> _userManager;
        public readonly RoleManager<IdentityRole<int>> _roleManager;

    

       public IdentityRepository(ApplicationDbContext dbContext,  UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, SignInManager<User> signInManager)
          {
            _dbcontext=dbContext;
            _userManager = userManager;
            _roleManager= roleManager;
            _signinManager =signInManager;
        }


         public async Task<List<GetUserDTO>> GetAllUsersAsync()
        {
            return await _dbcontext.Users.Select(u=> new GetUserDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email ?? string.Empty,
                UserName = u.UserName ?? string.Empty,
                PhoneNumber = u.PhoneNumber ?? string.Empty,
                Status = u.Status,
                UserAdded= u.UserAdded,
                
            }).ToListAsync();
        }

         public async Task AddUserAsync(AddUserDTO user)
        {
            
           User newuser = new User()
           {
               FirstName = user.FirstName,
               LastName = user.LastName,
               DateAdded =DateTime.UtcNow,
               Email = user.Email,
               EmailConfirmed = true,
               UserName = user.Email,
               PhoneNumber = user.PhoneNumber,
               Status = UserStatus.Active,
               UserAdded ="Admin"

           };


           var result = await _userManager.CreateAsync(newuser, user.Password);
           if (!result.Succeeded)
            {
                var errors = string.Join(" User Creation Error: ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create user: {errors}");
            }
            if (!string.IsNullOrEmpty(user.Role))
            {
                await _userManager.AddToRoleAsync(newuser, user.Role);
            }
        }
        
         public async Task<bool> LoginAsync(LoginDTO dto)
            {
            //      // Load default roles if no roles exist
            // if (!await _roleManager.Roles.AnyAsync())
            // {
            //     await _roleManager.CreateAsync(new IdentityRole<int> { Name = UserRole.Admin.ToString() });
            //     await _roleManager.CreateAsync(new IdentityRole<int> { Name = UserRole.Customer.ToString() });
            // }


            // // Load Sample data if no users exist
            // var AdminUser = new User()
            // {
            //     FirstName = "Admin",
            //     LastName = "User",
            //     Email = "admin@example.com",
            //     PhoneNumber = "1234567890",
            //     UserName = "admin@example.com",
            //     EmailConfirmed = true,
            // };

            // var existing = await _userManager.FindByEmailAsync(AdminUser.Email);
            // if (existing == null)
            // {
            //     await _userManager.CreateAsync(AdminUser, "Admin@123");
            // }

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return false;
            }

            if (user.Status != UserStatus.Active)
            {
                return false;
            }

            var result = await _signinManager.PasswordSignInAsync(
                user.UserName ?? dto.Email,
                dto.Password,
                dto.RememberMe,
                lockoutOnFailure: true
            );

            return result.Succeeded;
            }
        public async Task LogoutAsync()
        {
            await _signinManager.SignOutAsync();
        }
    }
}
          
    

