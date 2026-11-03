using Microsoft.AspNetCore.Identity;
using Domain.ValueObjects;


namespace Infrastructure.Identity
{
    public class User:IdentityUser<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public DateTime DateAdded { get; set; }
        public string UserAdded { get; set; } = string.Empty;
        public UserStatus Status { get; set; }
    }
}
