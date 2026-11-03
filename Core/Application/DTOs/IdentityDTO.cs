using Domain.ValueObjects;
namespace Application.DTOs
{
    public class GetUserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public UserStatus Status{get;set;}
        public string UserAdded { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

    }
    public class AddUserDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
    public class LoginDTO
    {
         public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe{get;set;}

    }
}