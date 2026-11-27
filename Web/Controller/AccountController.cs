using Application.DTOs;
using Application.Services.IdentityServices;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class er : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<er> _logger;

        public er(IIdentityService identityService, ILogger<er> logger)
        {
            _identityService = identityService;
            _logger = logger;
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password, [FromForm] bool rememberMe, [FromForm] string? returnUrl)
        {
            try
            {
                var result = await _identityService.LoginAsync(new LoginDTO
                {
                    Email = email,
                    Password = password,
                    RememberMe = rememberMe
                });

                if (result)
                {
                    var redirect = !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
                        ? returnUrl
                        : "/";
                    return Redirect(redirect);
                }

                return RedirectToLoginWithError("Invalid email or password.", returnUrl);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Login could not be completed.");
                return RedirectToLoginWithError(
                    "Sign-in is temporarily unavailable. Please try again later.",
                    returnUrl);
            }
        }

        private IActionResult RedirectToLoginWithError(string message, string? returnUrl)
        {
            var errorUrl = $"/account/login?error={Uri.EscapeDataString(message)}";
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                errorUrl += $"&returnUrl={Uri.EscapeDataString(returnUrl)}";
            }

            return Redirect(errorUrl);
        }

        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _identityService.LogOutAsync();
            return Redirect("/account/login");
        }
    }
}