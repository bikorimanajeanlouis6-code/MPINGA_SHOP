namespace Application.Interfaces
{
    /// <summary>
    /// Provides access to the current authenticated user's information.
    /// All properties read directly from claims - no async loading needed.
    /// Usage: @inject IUserContext UserContext, then UserContext.Email, UserContext.Id, etc.
    /// </summary>
    public interface IUserContext
    {
        /// <summary>
        /// Gets the current user's ID, or null if not authenticated.
        /// </summary>
        int? Id { get; }

        /// <summary>
        /// Gets whether the current user is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Gets the current user's email, or empty string if not authenticated.
        /// </summary>
        string Email { get; }

        /// <summary>
        /// Gets the current user's first name, or empty string if not authenticated.
        /// </summary>
        string FirstName { get; }

        /// <summary>
        /// Gets the current user's last name, or empty string if not authenticated.
        /// </summary>
        string LastName { get; }

        /// <summary>
        /// Gets the full name (FirstName LastName)
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Gets the avatar initials (first letter of first name + first letter of last name)
        /// </summary>
        string Initials { get; }
        bool  IsAdmin {get;}
        bool IsInstructor{get;}
        
    }
}