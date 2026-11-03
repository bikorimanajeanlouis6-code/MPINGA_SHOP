using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.Identity
{
    
        public static class Authentication
    {
        public static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
             services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
            }).AddIdentityCookies();

            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireNonAlphanumeric=true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 10;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;


                options.Lockout.MaxFailedAccessAttempts = 3;
            })
            .AddRoles<IdentityRole<int>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders()
            
           

            .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>();
             services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromDays(6);
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "Attend";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.Lax;
            });
            
            return services;

        }
    }
}